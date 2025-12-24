using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ProjectUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7227/api/News";

        public NewsController()
        {
            _httpClient = new HttpClient();
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không thể lấy dữ liệu tin tức.";
                return View(new List<NewsDto>());
            }
            var json = await response.Content.ReadAsStringAsync();
            var newsList = JsonConvert.DeserializeObject<List<NewsDto>>(json);
            return View(newsList);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var json = await response.Content.ReadAsStringAsync();
            var news = JsonConvert.DeserializeObject<NewsDto>(json);
            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNewsDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            ViewBag.Error = "Thêm tin tức thất bại.";
            return View(model);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();
            var json = await response.Content.ReadAsStringAsync();
            var news = JsonConvert.DeserializeObject<NewsDto>(json);
            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateNewsDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            ViewBag.Error = "Cập nhật tin tức thất bại.";
            return View(model);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();
            var json = await response.Content.ReadAsStringAsync();
            var news = JsonConvert.DeserializeObject<NewsDto>(json);
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }
    }

    // DTOs cho News (có thể chuyển sang Models nếu muốn)
    public class NewsDto
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime PostedDate { get; set; }
        public string Author { get; set; }
    }
    public class CreateNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
    }
    public class UpdateNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
    }
} 