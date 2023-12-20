using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oceans_DataAccess.Data;
using Oceans_DataAccess.Repository.IRepository;
using Oceans_Models;
using Oceans_Models.ViewModels;
using Oceans_Utility;

namespace OceansBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CompanyController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            List<Company> CompanyList = _unitofwork.Company.GetAll().ToList();
            return View(CompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = CategoryList;
            // ViewData["CategoryList"] = CategoryList
           
            if(id == null || id == 0)
            {
                // Create
                return View(new Company());
            }
            else
            {
                // Update
                Company companyObj = _unitofwork.Company.Get(u=>u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            // server side validation
            if(ModelState.IsValid)
            { 
                if(CompanyObj.Id == 0)
                {
                    // Add
                    _unitofwork.Company.Add(CompanyObj);
                }
                else
                {
                    //Update
                    _unitofwork.Company.Update(CompanyObj);
                }
                _unitofwork.Save();

                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }
        }

     
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyList = _unitofwork.Company.GetAll().ToList();
            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitofwork.Company.Get(u=>u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitofwork.Company.Remove(CompanyToBeDeleted);
            _unitofwork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
