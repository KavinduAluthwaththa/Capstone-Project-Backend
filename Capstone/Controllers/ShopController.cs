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

        // GET: api/shop/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShopById(int id)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.ShopID == id);

            if (shop == null)
            {
                return NotFound(new { message = "Shop not found" });
            }

            return Ok(shop);
        }

        // POST: api/shop
        [HttpPost]
        public async Task<IActionResult> AddShop([FromBody] Shop shop)
        {
            if (shop == null)
            {
                return BadRequest(new { message = "Shop data is required." });
            }

            try
            {
                _context.Shops.Add(shop);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetShopById), new { id = shop.ShopID }, shop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the shop.", error = ex.Message });
            }
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
                existingShop.Name = shop.Name;
                existingShop.PhoneNumber = shop.PhoneNumber;
                existingShop.Location = shop.Location;

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
