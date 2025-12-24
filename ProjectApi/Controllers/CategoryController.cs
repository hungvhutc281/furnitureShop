using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.Dto;
using ProjectApi.Model;

namespace ProjectApi.Controllers
{
    [Route("api/admin/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        public CategoryController(FurnitureStoreContext context)
        {
            _context = context;
        }
        // GET: api/admin/category
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = _context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToList();

            return Ok(categories);
        }

        // GET: api/admin/category/5
        [HttpGet("{id}")]
        public ActionResult<CategoryDTO> GetCategoryById(int id)
        {
            var category = _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .FirstOrDefault();

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/admin/category
        [HttpPost]
        public ActionResult<CategoryDTO> CreateCategory(CategoryDTO dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            dto.CategoryId = category.CategoryId;
            return CreatedAtAction(nameof(GetCategoryById), new { id = dto.CategoryId }, dto);
        }

        // PUT: api/admin/category/5
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, CategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
                return NotFound();

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/admin/category/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

