using Capstone.Models.Entities;
using Capstone.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/shop
        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var shops = await _context.Shops.ToListAsync();
            return Ok(shops);
        }

        // GET: api/shop/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetShopById(String email)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.Email == email);

            if (shop == null)
            {
                return NotFound(new { message = "Shop not found" });
            }

            return Ok(shop);
        }

        // PUT: api/shop/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromBody] Shop shop)
        {
            if (shop == null)
            {
                return BadRequest(new { message = "Shop data is required." });
            }

            var existingShop = await _context.Shops.FindAsync(id);
            if (existingShop == null)
            {
                return NotFound(new { message = "Shop not found" });
            }

            try
            {
                // Only update fields that have been provided
                if (!string.IsNullOrEmpty(shop.Name))
                {
                    existingShop.Name = shop.Name;
                }

                // For numeric fields, check if it has a non-default value
                if (shop.PhoneNumber != 0)
                {
                    existingShop.PhoneNumber = shop.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(shop.Location))
                {
                    existingShop.Location = shop.Location;
                }
                
                // Email field can also be updated if provided
                if (!string.IsNullOrEmpty(shop.Email))
                {
                    existingShop.Email = shop.Email;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Shop updated successfully.", shop = existingShop });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the shop.", error = ex.Message });
            }
        }

        // DELETE: api/shop/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound(new { message = "Shop not found" });
            }

            try
            {
                _context.Shops.Remove(shop);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Shop deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the shop.", error = ex.Message });
            }
        }
    }
}
