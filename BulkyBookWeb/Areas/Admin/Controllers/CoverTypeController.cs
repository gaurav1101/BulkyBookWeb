using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private IUnitOfWork _db;
        public CoverTypeController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> _covertypeList = (IEnumerable<CoverType>)_db.covertype.GetAll();
            return View(_covertypeList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            //if (obj.Name == obj..ToString())
            //{
            //    ModelState.AddModelError("Name", "Name and Display number cannot be same");
            //}
            if (ModelState.IsValid)
            {
                _db.covertype.Add(obj);
                _db.save();
                TempData["Success"] = "Created Successfully!!!";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            var coverType = _db.covertype.GetFirstOrDefault(c => c.id == id);
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "Name and Display number cannot be same");
            //}
            if (ModelState.IsValid)
            {
                _db.covertype.update(obj);
                _db.save();
                TempData["Success"] = "Updated Successfully!!!";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            var category = _db.covertype.GetFirstOrDefault(c => c.id == id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.covertype.GetFirstOrDefault(c => c.id == id);
            if (category == null)
            {
                return NotFound();
            }
            _db.covertype.Remove(category);
            _db.save();
            TempData["Success"] = "Deleted Successfully!!!";
            return RedirectToAction("Index");
        }
    }
}

