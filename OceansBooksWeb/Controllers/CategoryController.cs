using Microsoft.AspNetCore.Mvc;
using Oceans_DataAccess.Data;
using Oceans_Models;

namespace OceansBooksWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
           List<Category>CategoryList = _db.Categories.ToList();
            return View(CategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // Custom Validation
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name","The Display Order cannot exactly match the Name." );
            }

            // server side validation
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id ==0)
            {
                return NotFound();
            }

            // three way to edit DB
            Category? category = _db.Categories.Find(id);
            //Category? category2 = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Category? category3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();

                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // three way to edit DB
            Category? category = _db.Categories.Find(id);
            //Category? category2 = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Category? category3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();

            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
