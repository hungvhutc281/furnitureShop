using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 