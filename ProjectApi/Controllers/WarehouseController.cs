using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.Dto;
using ProjectApi.Model;

namespace ProjectApi.Controllers
{
    [Route("api/admin/warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        public WarehouseController(FurnitureStoreContext context)
        {
            _context = context;
        }
        // GET: api/admin/Warehouse
        [HttpGet]
        public ActionResult<IEnumerable<WarehouseDTO>> GetAllWarehouses()
        {
            var warehouses = _context.Warehouses
                .Select(c => new WarehouseDTO
                {
                    WarehouseId = c.WarehouseId,
                    WarehouseName = c.WarehouseName,
                    Location = c.Location,
                    Phone = c.Phone,
                    Capacity = c.Capacity
                   
                })
                .ToList();

            return Ok(warehouses);
        }

        // GET: api/admin/Warehouse/5
        [HttpGet("{id}")]
        public ActionResult<WarehouseDTO> GetWarehouseById(int id)
        {
            var warehouse = _context.Warehouses
               .Where(c => c.WarehouseId == id)
                .Select(c => new WarehouseDTO
                {
                    WarehouseId = c.WarehouseId,
                    WarehouseName = c.WarehouseName,
                    Location = c.Location,
                    Phone = c.Phone,
                    Capacity = c.Capacity
                })
                .FirstOrDefault();

            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        // POST: api/admin/Warehouse
        [HttpPost]
        public ActionResult<WarehouseDTO> CreateWarehouse(WarehouseDTO dto)
        {
            var warehouse = new Warehouse
            {
                WarehouseName = dto.WarehouseName,
                Location = dto.Location,
                Phone = dto.Phone,
                Capacity = dto.Capacity
            };

            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();

            dto.WarehouseId = warehouse.WarehouseId;
            return CreatedAtAction(nameof(GetAllWarehouses), new { id = dto.WarehouseId }, dto);
        }

        // PUT: api/admin/Warehouse/5
        [HttpPut("{id}")]
        public IActionResult UpdateWarehouse(int id,WarehouseDTO dto)
        {
            var warehouse = _context.Warehouses.FirstOrDefault(c => c.WarehouseId == id);
            if (warehouse == null)
                return NotFound();

            warehouse.WarehouseName = dto.WarehouseName;
            warehouse.Phone = dto.Phone;
            warehouse.Capacity = dto.Capacity;
            warehouse.Location = dto.Location;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/admin/Warehouse/5
        [HttpDelete("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {
            var warehouse = _context.Warehouses.FirstOrDefault(c => c.WarehouseId == id);
            if (warehouse == null)
                return NotFound();

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
