
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _db;
        public CategoryController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> _categoryList = (IEnumerable<Category>)_db.category.GetAll();
            return View(_categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display number cannot be same");
            }
            if (ModelState.IsValid)
            {
                _db.category.Add(obj);
                _db.save();
                TempData["Success"] = "Created Successfully!!!";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            var category = _db.category.GetFirstOrDefault(c => c.id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display number cannot be same");
            }
            if (ModelState.IsValid)
            {
                _db.category.update(obj);
                _db.save();
                TempData["Success"] = "Updated Successfully!!!";
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            var category = _db.category.GetFirstOrDefault(c => c.id == id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.category.GetFirstOrDefault(c => c.id == id);
            if (category == null)
            {
                return NotFound();
            }
            _db.category.Remove(category);
            _db.save();
            TempData["Success"] = "Deleted Successfully!!!";
            return RedirectToAction("Index");
        }
    }
}
