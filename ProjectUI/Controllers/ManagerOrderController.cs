using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    public class ManagerOrderController : Controller
    {
        // GET: ManagerOrderController
        // Action quản lý đơn hàng
        public IActionResult OrderManagement()
        {
            return View();
        }

        // GET: ManagerOrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagerOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagerOrderController/Create
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

        // GET: ManagerOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagerOrderController/Edit/5
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

        // GET: ManagerOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagerOrderController/Delete/5
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
