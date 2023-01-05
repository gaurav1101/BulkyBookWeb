using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using BulkyBookWeb.Repository;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Costumer.Controllers
{
    [Area("Costumer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable < Product > list= _db.productRepository.GetAll(includeProperties: "Category,CoverType");
            return View(list);
        }

        public IActionResult Details(int produtId)
        {
            ShoppingCart cart = new ShoppingCart
            {
                ProductId= produtId,
                product = _db.productRepository.GetFirstOrDefault(p => p.id == produtId, includeProperties: "Category,CoverType"),
                count = 1
            };
            return View(cart);
        }
      
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart obj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            obj.ApplicationUserId = claim.Value;
            ShoppingCart cart = _db.shoppingCartRepository.GetFirstOrDefault(s => s.ProductId == obj.ProductId
            && s.ApplicationUserId==obj.ApplicationUserId);
            if (cart == null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart , _db.shoppingCartRepository.GetAll(u=>u.ApplicationUserId==claim.Value).ToList().Count);
                _db.shoppingCartRepository.Add(obj);
            }
            else
            {
                _db.shoppingCartRepository.IncrementCount(cart, obj.count);
            }
            _db.save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}