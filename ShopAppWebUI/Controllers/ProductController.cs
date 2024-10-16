using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopAppWebUI.Controllers
{
    public class ProductController : Controller
    {
        [Authorize]
        public IActionResult GetAll()

        {
            return View();
        }
    }
}
