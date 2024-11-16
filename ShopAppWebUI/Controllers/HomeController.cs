using Microsoft.AspNetCore.Mvc;
using ShopAppWebUI.Models;
using ShopAppWebUI.Models.DTOs;
using System.Diagnostics;

namespace ShopAppWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm="", int collectionId=0, double? minPrice = null, double? maxPrice = null)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, collectionId, minPrice, maxPrice);
            IEnumerable<Collection> collections = await _homeRepository.Collections();

            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Collections = collections,
                STerm = sterm,
                CollectionId = collectionId,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return View(productModel);
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
