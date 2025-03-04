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
    public class CropShopController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CropShopController(AppDbContext context)
        {
            _context = context;
        }

        // Get all CropShop entries
        [HttpGet]
        public async Task<IActionResult> GetAllCropShops()
        {
            var cropShops = await _context.CropShops
                .Include(cs => cs.Crop)
                .Include(cs => cs.Shop)
                .ToListAsync();
            return Ok(cropShops);
        }

        // Get a CropShop by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCropShopById(int id)
        {
            var cropShop = await _context.CropShops
                .Include(cs => cs.Crop)
                .Include(cs => cs.Shop)
                .FirstOrDefaultAsync(cs => cs.csid == id);

            if (cropShop == null)
            {
                return NotFound(new { message = "CropShop record not found." });
            }

            return Ok(cropShop);
        }

        // Add a new CropShop record
        [HttpPost]
        public async Task<IActionResult> AddCropShop([FromBody] CropShop cropShop)
        {
            if (cropShop == null)
            {
                return BadRequest(new { message = "CropShop data is required." });
            }

            if (cropShop.CropID <= 0 || cropShop.ShopID <= 0 || string.IsNullOrWhiteSpace(cropShop.Date))
            {
                return BadRequest(new { message = "CropID, ShopID, and Date are required." });
            }

            try
            {
                _context.CropShops.Add(cropShop);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCropShopById), new { id = cropShop.csid }, cropShop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the CropShop record.", error = ex.Message });
            }
        }

        // Update an existing CropShop record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCropShop(int id, [FromBody] CropShop cropShop)
        {
            if (cropShop == null)
            {
                return BadRequest(new { message = "CropShop data is required." });
            }

            var existingCropShop = await _context.CropShops.FindAsync(id);
            if (existingCropShop == null)
            {
                return NotFound(new { message = "CropShop record not found." });
            }

            if (cropShop.CropID <= 0 || cropShop.ShopID <= 0 || string.IsNullOrWhiteSpace(cropShop.Date))
            {
                return BadRequest(new { message = "CropID, ShopID, and Date are required." });
            }

            try
            {
                existingCropShop.CropID = cropShop.CropID;
                existingCropShop.ShopID = cropShop.ShopID;
                existingCropShop.Date = cropShop.Date;

                await _context.SaveChangesAsync();

                return Ok(new { message = "CropShop record updated successfully.", cropShop = existingCropShop });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the CropShop record.", error = ex.Message });
            }
        }

        // Delete a CropShop record by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCropShop(int id)
        {
            var cropShop = await _context.CropShops.FindAsync(id);
            if (cropShop == null)
            {
                return NotFound(new { message = "CropShop record not found." });
            }

            _context.CropShops.Remove(cropShop);
            await _context.SaveChangesAsync();

            return Ok(new { message = "CropShop record deleted successfully." });
        }
    }
}
