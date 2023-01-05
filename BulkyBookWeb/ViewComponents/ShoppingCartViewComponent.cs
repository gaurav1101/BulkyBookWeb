using BulkyBook.Utility;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _db;
        public ShoppingCartViewComponent(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart)!=null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _db.shoppingCartRepository
                        .GetAll(u=>u.ApplicationUserId==claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
   
}
