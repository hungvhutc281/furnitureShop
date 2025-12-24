using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Model;
using ProjectApi.Dto;
using System.Linq;
using System.Threading.Tasks;
using FurnitureStore.Models;
using Microsoft.Extensions.Logging; // thêm log

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;
        private readonly ILogger<CartController> _logger; 

        public CartController(FurnitureStoreContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCart(int userId)
        {
            _logger.LogInformation($"Fetching cart for userId: {userId}");
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid userId provided: {UserId}", userId);
                return BadRequest("Invalid user ID.");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                _logger.LogInformation($"No cart found for userId: {userId}. Returning empty cart.");
                return new CartDto { UserId = userId, Items = new List<CartItemDto>() };
            }

            var cartDto = new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown Product",
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };

            _logger.LogInformation($"Cart retrieved for userId: {userId}. Item count: {cartDto.Items.Count}");
            return cartDto;
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            _logger.LogInformation($"Adding item to cart for userId: {dto.UserId}, productId: {dto.ProductId}, quantity: {dto.Quantity}");

            if (dto.UserId <= 0 || dto.ProductId <= 0 || dto.Quantity <= 0)
            {
                _logger.LogWarning("Invalid input data: {@CartItemDto}", dto);
                return BadRequest("Invalid input data.");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null)
            {
                _logger.LogInformation($"Creating new cart for userId: {dto.UserId}");
                cart = new Cart { UserId = dto.UserId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                _logger.LogWarning($"Product not found: {dto.ProductId}");
                return NotFound("Product not found.");
            }

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item != null)
            {
                item.Quantity += dto.Quantity;
                _logger.LogInformation($"Updated quantity for productId: {dto.ProductId}. New quantity: {item.Quantity}");
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price // Use price from Product table
                });
                _logger.LogInformation($"Added new item to cart: productId: {dto.ProductId}, quantity: {dto.Quantity}, unitPrice: {product.Price}");
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Cart updated for userId: {dto.UserId}. Total items: {cart.CartItems.Count}");
            return Ok();
        }

        // POST: api/Cart/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemDto dto)
        {
            _logger.LogInformation($"Updating cart item: cartItemId: {dto.CartItemId}, quantity: {dto.Quantity}");

            if (dto.CartItemId <= 0 || dto.Quantity <= 0)
            {
                _logger.LogWarning("Invalid input data: {@CartItemDto}", dto);
                return BadRequest("Invalid input data.");
            }

            var item = await _context.CartItems.FindAsync(dto.CartItemId);
            if (item == null)
            {
                _logger.LogWarning($"Cart item not found: {dto.CartItemId}");
                return NotFound("Cart item not found.");
            }

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Cart item updated: cartItemId: {dto.CartItemId}, new quantity: {dto.Quantity}");
            return Ok();
        }

        // POST: api/Cart/remove
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartItemDto dto)
        {
            _logger.LogInformation($"Removing cart item: cartItemId: {dto.CartItemId}");

            if (dto.CartItemId <= 0)
            {
                _logger.LogWarning("Invalid cart item ID: {CartItemId}", dto.CartItemId);
                return BadRequest("Invalid cart item ID.");
            }

            var item = await _context.CartItems.FindAsync(dto.CartItemId);
            if (item == null)
            {
                _logger.LogWarning($"Cart item not found: {dto.CartItemId}");
                return NotFound("Cart item not found.");
            }

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Cart item removed: cartItemId: {dto.CartItemId}");
            return Ok();
        }

       
    }
}