using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private IUnitOfWork _db;
		public OrderController(IUnitOfWork db)
		{
				_db= db;
		}
		public IActionResult Index()
		{
            return View();
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			IEnumerable<OrderHeader> _oredrList;
            _oredrList = _db.orderHeaderRepository.GetAll(includeProperties:"ApplicationUser");
			return Json(new { data = _oredrList });
		}

		public IActionResult Details(int id)
		{
			OrderVM orderVM = new OrderVM()
			{
				orderHeader = _db.orderHeaderRepository.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser"),
				orderDetails = _db.orderDetailRepository.GetAll(u => u.OrderId == id, includeProperties: "Product")
			};
            return View(orderVM);
        }
    }
}
