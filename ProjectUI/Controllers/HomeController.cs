using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ProjectUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
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

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
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

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
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

        // GET: HomeController/About
        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Tintuc()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7227/api/News");
            var newsList = new List<NewsDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                newsList = JsonConvert.DeserializeObject<List<NewsDto>>(json);
                newsList = newsList.OrderByDescending(n => n.PostedDate).ToList();
            }
            return View(newsList);
        }

        public async Task<IActionResult> ChitietTin(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://localhost:7227/api/News/{id}");
            NewsDto news = null;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                news = JsonConvert.DeserializeObject<NewsDto>(json);
            }
            if (news == null) return NotFound();
            return View(news);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public class NewsDto
        {
            public int NewsID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Image { get; set; }
            public System.DateTime PostedDate { get; set; }
            public string Author { get; set; }
        }
    }
}
