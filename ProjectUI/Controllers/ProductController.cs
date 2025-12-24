using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProjectUI.Models;
using System.Linq;

namespace ProjectUI.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Gọi API lấy thông tin sản phẩm
            var httpClient = new HttpClient();
            string apiBase = "https://localhost:7227/api/admin";
            var product = await httpClient.GetFromJsonAsync<ProductDTO>($"{apiBase}/product/{id}");
            if (product == null) return NotFound();

            // Gọi API lấy ảnh chi tiết sản phẩm
            var images = await httpClient.GetFromJsonAsync<List<ProductImageDTO>>($"{apiBase}/productimage");
            var productImages = images?.Where(i => i.ProductId == id).Select(i => i.ImageUrl).ToList() ?? new List<string>();
            // Thêm ảnh đại diện chính lên đầu nếu có
            if (!string.IsNullOrEmpty(product.Image))
                productImages.Insert(0, product.Image);

            // Gọi API lấy tên danh mục
            string categoryName = "";
            var category = await httpClient.GetFromJsonAsync<CategoryDTO>($"{apiBase}/category/{product.CategoryId}");
            if (category != null && !string.IsNullOrEmpty(category.CategoryName))
                categoryName = category.CategoryName;

            var model = new ProductDetailViewModel
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                Material = product.Material,
                Dimensions = product.Dimensions,
                CategoryName = categoryName,
                StockQuantity = product.StockQuantity,
                ImageUrls = productImages
            };
            return View(model);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
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

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
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

        // DTO cho Product
        public class ProductDTO
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int CategoryId { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public string Description { get; set; }
            public string Material { get; set; }
            public string Dimensions { get; set; }
            public string Image { get; set; }
        }

        // DTO cho ProductImage
        public class ProductImageDTO
        {
            public int ImageId { get; set; }
            public int ProductId { get; set; }
            public string ImageUrl { get; set; }
        }

        // DTO cho Category
        public class CategoryDTO
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
        }
    }
}
