using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oceans_DataAccess.Data;
using Oceans_DataAccess.Repository.IRepository;
using Oceans_Models;
using Oceans_Models.ViewModels;

namespace OceansBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ProductList = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();
            return View(ProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = CategoryList;
            // ViewData["CategoryList"] = CategoryList
            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }), 

                Product = new Product()
            };
            if(id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                // Update
                productVM.Product = _unitofwork.Product.Get(u=>u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            // server side validation
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if( file != null )
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagepath = Path.Combine(wwwRootPath,obj.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagepath))
                        {
                            System.IO.File.Delete(oldImagepath);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create ))
                    {
                        file.CopyTo(filestream);
                    }

                    obj.Product.ImageUrl = @"\Images\Product\" + fileName;
                }

                if(obj.Product.Id == 0)
                {
                    // Add
                    _unitofwork.Product.Add(obj.Product);
                }
                else
                {
                    //Update
                    _unitofwork.Product.Update(obj.Product);
                }
                _unitofwork.Save();

                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(obj);
            }
        }

     
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = ProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitofwork.Product.Get(u=>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            var oldImagepath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagepath))
            {
                System.IO.File.Delete(oldImagepath);
            }

            _unitofwork.Product.Remove(productToBeDeleted);
            _unitofwork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
