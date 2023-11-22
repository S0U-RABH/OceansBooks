using Microsoft.AspNetCore.Mvc;
using Oceans_DataAccess.Repository.IRepository;
using Oceans_Models;
using System.Diagnostics;

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
            Product Products = _unitOfWork.Product.Get(u =>u.Id == ProductID, includeProperties: "Category");
            return View(Products);
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
