using Microsoft.AspNetCore.Mvc;
using ShopAppWebUI.Models;

namespace ShopAppWebUI.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            var photos = new List<Photos>
            {
                new Photos { Url = "https://drive.usercontent.google.com/download?id=16KqbjyimnLB0tw3Pz1ivW2IoGLRUQLfm"},
            };
            return View(photos);
        }
    }
}
