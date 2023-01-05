using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _db;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> _categoryList = (IEnumerable<Category>)_db.category.GetAll();
            return View(_categoryList);
        }

        
        public IActionResult Upsert(int? id)
        {
            ProductVM product = new ProductVM
            {
                categoryList = _db.category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.id.ToString(),
                }),
                coverTypeList = _db.covertype.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.id.ToString(),
                }),
                product = new()
            };

            if (id==null || id == 0)
            {
                //ViewBag.categoryList = categoryList;
                //ViewData["coverTypeList"] = coverTypeList;
                return View(product);
            }
            else
            {
                product.product = _db.productRepository.GetFirstOrDefault(p=>p.id==id);
                return View(product);
            }
            //return View(product);
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName=Guid.NewGuid().ToString();
                    var uploads=Path.Combine(wwwRootPath, @"images\products\");
                    var extension=Path.GetExtension(file.FileName);

                    if (obj.product.imgUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.imgUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var filestream=new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.product.imgUrl = @"images\products\" + fileName + extension;
                    if(obj.product.id != 0) 
                    {
                        _db.productRepository.Update(obj.product);
                    }
                    else
                    {
                        _db.productRepository.Add(obj.product);
                    }
                    _db.save();
                    TempData["Success"] = "Created Successfully!!!";
                    return RedirectToAction("Index");
                }
            }
            return View(obj);
        }


        #region for API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var apiData=_db.productRepository.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data=apiData });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var objDelete = _db.productRepository.GetFirstOrDefault(c => c.id == id);
            if (objDelete == null)
            {
                return Json(new { succes = false, message = "Error while deleting" });
            }
            if (objDelete.imgUrl != null)
            {
                var imagePath = Path.Combine(wwwRootPath, objDelete.imgUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _db.productRepository.Remove(objDelete);
            _db.save();
            
            return Json(new { success = true,message="deleted successfully" });
        }
        #endregion
    }
}
