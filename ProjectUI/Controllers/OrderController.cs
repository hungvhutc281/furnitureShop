using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }
    }
} 