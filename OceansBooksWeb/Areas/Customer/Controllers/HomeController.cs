using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oceans_DataAccess.Repository.IRepository;
using Oceans_Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OceansBooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductsList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(ProductsList);
        }
        public IActionResult Details(int ProductID)
        {
            ShoppingCart cart = new ShoppingCart() {
                Product = _unitOfWork.Product.Get(u => u.Id == ProductID, includeProperties: "Category"),
                Count = 1,
                ProductId = ProductID
            };
               
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                //shopping cart exist
                //Update 
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);

            }
            else
            {
                //Add
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            TempData["success"] = "Cart Updated Successfully";
            _unitOfWork.Save();

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
