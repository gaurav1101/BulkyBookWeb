using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> company=_unitOfWork.companyRepository.GetAll();
            return View(company);
        }

        public IActionResult Upsert(int? id)
        {
            Company company=new Company();
            if (id > 0)
            {
                 company = _unitOfWork.companyRepository.GetFirstOrDefault(p => p.Id == id);
                return View(company);
            }
            else
            {
                return View(company);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid) 
            { 
                 if (obj.Id == null || obj.Id==0)
                 {
                    _unitOfWork.companyRepository.Add(obj);
                 }
                else
                {
                    _unitOfWork.companyRepository.Update(obj);
                }

                _unitOfWork.save();
                TempData["Success"] = "Created Successfully!!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region for API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var apiData = _unitOfWork.companyRepository.GetAll();
            return Json(new { data = apiData });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
          
            var objDelete = _unitOfWork.companyRepository.GetFirstOrDefault(c => c.Id == id);
            if (objDelete == null)
            {
                return Json(new { succes = false, message = "Error while deleting" });
            }
            else
            {
                _unitOfWork.companyRepository.Remove(objDelete);
                _unitOfWork.save();
            }

            return Json(new { success = true, message = "deleted successfully" });
        }
        #endregion
    }
}
