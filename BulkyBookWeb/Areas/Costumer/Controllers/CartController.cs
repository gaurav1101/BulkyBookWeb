using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using BulkyBookWeb.Repository;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Costumer.Controllers
{
    [Area("Costumer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _db;
		public CartController(IUnitOfWork db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM cartVM = new ShoppingCartVM()
            {
                ListCart = _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "product"),
                orderHeader = new BulkyBook.Models.OrderHeader()
            };
            cartVM.orderHeader.OrderTotal = calculatePrice(cartVM);
			  return View(cartVM);
		}

		public IActionResult plus(int itemid)
        {
            var cart = _db.shoppingCartRepository.GetFirstOrDefault(u=>u.ProductId== itemid);
            _db.shoppingCartRepository.IncrementCount(cart,1);
            _db.save();
            return RedirectToAction(nameof(Index));
        }

		public IActionResult Summary()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM cartVM = new ShoppingCartVM()
			{
				ListCart = _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "product"),
				orderHeader = new BulkyBook.Models.OrderHeader()
			};
			var applicationUser = _db.applicationUserRepository.GetFirstOrDefault(u => u.Id == claim.Value);
			cartVM.orderHeader.Name = applicationUser.UserName;
			cartVM.orderHeader.PhoneNumber = applicationUser.PhoneNumber;
			cartVM.orderHeader.City = applicationUser.City;
			cartVM.orderHeader.State = applicationUser.State;
			cartVM.orderHeader.PostalCode = applicationUser.PostalCode;
			cartVM.orderHeader.OrderTotal = calculatePrice(cartVM);
				
			return View(cartVM);
		}

		[HttpPost]
		public IActionResult Summary(ShoppingCartVM shoppingCartVM)
		{
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				shoppingCartVM.ListCart = _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value,
					includeProperties: "product");


				shoppingCartVM.orderHeader.OrderDate = System.DateTime.Now;
				shoppingCartVM.orderHeader.ApplicationUserId = claim.Value;

				shoppingCartVM.orderHeader.OrderTotal = calculatePrice(shoppingCartVM);
				
				ApplicationUser applicationUser = _db.applicationUserRepository.GetFirstOrDefault(u => u.Id == claim.Value);

				if (applicationUser.CompanyId.GetValueOrDefault() == 0)
				{
					shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
					shoppingCartVM.orderHeader.OrderStatus = SD.StatusPending;
				}
				else
				{
					shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
					shoppingCartVM.orderHeader.OrderStatus = SD.StatusApproved;
				}

				_db.orderHeaderRepository.Add(shoppingCartVM.orderHeader);
				_db.save();
				foreach (var cart in shoppingCartVM.ListCart)
				{
					OrderDetail orderDetail = new()
					{
						ProductId = cart.ProductId,
						OrderId = shoppingCartVM.orderHeader.Id,
						Price = shoppingCartVM.orderHeader.OrderTotal,
						Count = cart.count
					};
					_db.orderDetailRepository.Add(orderDetail);
					_db.save();
				}


				if (applicationUser.CompanyId.GetValueOrDefault() == 0)
				{
					//stripe settings 
					var domain = "https://localhost:44300/";
					var options = new SessionCreateOptions
					{
						PaymentMethodTypes = new List<string>
				{
				  "card",
				},
						LineItems = new List<SessionLineItemOptions>(),
						Mode = "payment",
						SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartVM.orderHeader.Id}",
						CancelUrl = domain + $"customer/cart/index",
					};

					foreach (var item in shoppingCartVM.ListCart)
					{

						var sessionLineItem = new SessionLineItemOptions
						{
							PriceData = new SessionLineItemPriceDataOptions
							{
								UnitAmount = (long)(item.product.Price * 1),//20.00 -> 2000
								Currency = "usd",
								ProductData = new SessionLineItemPriceDataProductDataOptions
								{
									Name = item.product .Title
								},

							},
							Quantity = item.count,
						};
						options.LineItems.Add(sessionLineItem);
					}

					var service = new SessionService();
					Session session = service.Create(options);
					_db.orderHeaderRepository.UpdateStripePaymentID(shoppingCartVM.orderHeader.Id, session.Id, session.PaymentIntentId);
					_db.save();
					Response.Headers.Add("Location", session.Url);
					return new StatusCodeResult(303);
				}

				else
				{
					return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.orderHeader.Id });
				}
			}
		}

		public IActionResult OrderConfirmation(int id)
		{
			OrderHeader orderHeader = _db.orderHeaderRepository.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
			if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
			{
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);
				//check the stripe status
				if (session.PaymentStatus.ToLower() == "paid")
				{
					_db.orderHeaderRepository.UpdateOrderStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
					_db.save();
				}
			}
			//_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book", "<p>New Order Created</p>");
			List<ShoppingCart> shoppingCarts = _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId ==
			orderHeader.ApplicationUserId).ToList();
			_db.shoppingCartRepository.RemoveRange(shoppingCarts);
			_db.save();
			return View(id);
		}
		public IActionResult minus(int itemid)
		{
			var cart = _db.shoppingCartRepository.GetFirstOrDefault(u => u.id == itemid);
            if (cart.count <= 0)
            {
				_db.shoppingCartRepository.Remove(cart);
                HttpContext.Session.SetInt32(SD.SessionCart, _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count);
            }
            else
            {
				_db.shoppingCartRepository.DecrementCount(cart, 1);
			}
			_db.save();
			return RedirectToAction(nameof(Index));
		}

		private double calculatePrice(ShoppingCartVM cartVM)
        {
            int count;
            double price, price50, price100,totalPrice=0;

            foreach(var d in cartVM.ListCart) 
            {
                if (d.count <= 50)
                {
					 price= d.product.Price * d.count;
                }
                else if(d.count<= 100) 
                {
					 price = d.product.Price50 * d.count;
                }
                else
                {
                     price = d.product.Price100 * d.count;
				}
				totalPrice += price;
			}
			return totalPrice;
		}

		public IActionResult Remove(int itemid)
		{
			var cart = _db.shoppingCartRepository.GetFirstOrDefault(u => u.id == itemid);
            _db.shoppingCartRepository.Remove(cart);
            HttpContext.Session.SetInt32(SD.SessionCart, _db.shoppingCartRepository.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count);
            _db.save();
			return RedirectToAction(nameof(Index));
		}

	}
}
