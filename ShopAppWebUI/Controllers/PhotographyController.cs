using Microsoft.AspNetCore.Mvc;

namespace ShopAppWebUI.Controllers
{
    public class PhotographyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
