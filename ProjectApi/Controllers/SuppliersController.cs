using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Models;
using ProjectApi.Model;
using ProjectApi.Dto;

namespace ProjectApi.Controllers
{
    [Route("api/admin/suppiler")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        public SuppliersController(FurnitureStoreContext context)
        {
            _context = context;
        }
        // GET: api/admin/suppiler
        [HttpGet]
        public ActionResult<IEnumerable<SuppilerDTO>> GetAllSuppiler()
        {
            var suppilers = _context.Suppliers
                .Select(c => new SuppilerDTO
                {
                    SupplierId  = c.SupplierId,
                    SupplierName = c.SupplierName,
                    ContactInfo = c.ContactInfo
                })
                .ToList();

            return Ok(suppilers);
        }

        // GET: api/admin/suppiler/5
        [HttpGet("{id}")]
        public ActionResult<SuppilerDTO> GetSuppilerById(int id)
        {
            var suppiler = _context.Suppliers
                .Where(c => c.SupplierId == id)
                .Select(c => new SuppilerDTO
                {
                    SupplierId = c.SupplierId,
                    SupplierName = c.SupplierName,
                    ContactInfo = c.ContactInfo
                })
                .FirstOrDefault();

            if (suppiler == null)
                return NotFound();

            return Ok(suppiler);
        }
        // POST: api/admin/suppiler
        [HttpPost]
        public ActionResult<SuppilerDTO> CreateSuppiler(SuppilerDTO dto)
        {
            var suppiler = new Supplier
            {
                SupplierName = dto.SupplierName,
                ContactInfo = dto.ContactInfo
            };

            _context.Suppliers.Add(suppiler);
            _context.SaveChanges();

            dto.SupplierId = suppiler.SupplierId;
            return CreatedAtAction(nameof(GetSuppilerById), new { id = dto.SupplierId }, dto);
        }

        // PUT: api/admin/suppiler/5
        [HttpPut("{id}")]
        public IActionResult UpdateSuppiler(int id, SuppilerDTO dto)
        {
            var suppiler = _context.Suppliers.FirstOrDefault(c => c.SupplierId == id);
            if (suppiler == null)
                return NotFound();

            suppiler.SupplierName = dto.SupplierName;
            suppiler.ContactInfo = dto.ContactInfo;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/admin/suppiler/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSuppiler (int id)
        {
            var suppiler = _context.Suppliers.FirstOrDefault(c => c.SupplierId == id);
            if (suppiler == null)
                return NotFound();

            _context.Suppliers.Remove(suppiler);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
