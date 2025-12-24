using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    public class ProductImageController : Controller
    {
        // GET: ProductImageController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductImageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductImageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductImageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductImageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductImageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductImageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductImageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
