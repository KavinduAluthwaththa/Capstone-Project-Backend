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
    public class GrowingCropController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GrowingCropController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GrowingCrop
        [HttpGet]
        public async Task<IActionResult> GetAllGrowingCrops()
        {
            var growingCrops = await _context.GrowingCrops
                .Include(g => g.Crop)
                .Include(g => g.Farmer)
                .ToListAsync();

            return Ok(growingCrops);
        }

        // GET: api/GrowingCrop/{FarmerID}
        [HttpGet("{FarmerID}")]
        public async Task<IActionResult> GetGrowingCrop(int FarmerID)
        {
            var growingCrops = await _context.GrowingCrops
                .Include(g => g.Crop)
                .Include(g => g.Farmer)
                .Where(g => g.FarmerID == FarmerID)
                .ToListAsync();

            if (!growingCrops.Any())
            {
                return NotFound(new { message = "No growing crop records found for this farmer" });
            }

            return Ok(growingCrops);
        }



        [HttpPost]
        public async Task<IActionResult> AddGrowingCrop([FromBody] GrowingCrop growingCrop)
        {
            if (growingCrop == null)
            {
                return BadRequest(new { message = "Growing crop data is required." });
            }

            try
            {
                _context.GrowingCrops.Add(growingCrop);
                await _context.SaveChangesAsync();

                return Created("", growingCrop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the growing crop.", error = ex.Message });
            }
        }

        // PUT: api/GrowingCrop/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrowingCrop(int id, [FromBody] GrowingCrop growingCrop)
        {
            if (growingCrop == null)
            {
                return BadRequest(new { message = "Growing crop data is required." });
            }

            var existingGrowingCrop = await _context.GrowingCrops.FindAsync(id);
            if (existingGrowingCrop == null)
            {
                return NotFound(new { message = "Growing crop record not found." });
            }

            try
            {
                existingGrowingCrop.CropID = growingCrop.CropID;
                existingGrowingCrop.FarmerID = growingCrop.FarmerID;
                existingGrowingCrop.Date = growingCrop.Date;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Growing crop updated successfully.", growingCrop = existingGrowingCrop });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the growing crop.", error = ex.Message });
            }
        }

        // DELETE: api/GrowingCrop/{id}
        [HttpDelete("{cfid}")]
        public async Task<IActionResult> DeleteGrowingCrop(int cfid)
        {
            var growingCrop = await _context.GrowingCrops.FindAsync(cfid);
            if (growingCrop == null)
            {
                return NotFound(new { message = "Growing crop record not found" });
            }

            try
            {
                _context.GrowingCrops.Remove(growingCrop);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Growing crop deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the growing crop.", error = ex.Message });
            }
        }
    }
}
