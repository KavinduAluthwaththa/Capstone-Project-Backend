using Capstone.Models.Entities;
using Capstone.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            return Ok(item);
        }

        // POST: api/items
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest(new { message = "Item data is required." });
            }

            try
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetItemById), new { id = item.ItemID }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the item.", error = ex.Message });
            }
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest(new { message = "Item data is required." });
            }

            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            try
            {
                existingItem.Amount = item.Amount;
                existingItem.Date = item.Date;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Item updated successfully.", item = existingItem });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the item.", error = ex.Message });
            }
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            try
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Item deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the item.", error = ex.Message });
            }
        }
    }
}
