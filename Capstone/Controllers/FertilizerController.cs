using Capstone.Models.Entities;
using Capstone.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FertilizerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FertilizerController(AppDbContext context)
        {
            _context = context;
        }

        // Get all fertilizers
        [HttpGet]
        public async Task<IActionResult> GetAllFertilizers()
        {
            var fertilizers = await _context.Fertilizers
                .Include(f => f.Crop)
                .Select(f => new
                {
                    f.FertilizerID,
                    f.FertilizerType,
                    f.RecommendedAmount,
                    CropName = f.Crop.CropName
                })
                .ToListAsync();

            return Ok(fertilizers);
        }

        // Add a new fertilizer
        [HttpPost]
        public async Task<IActionResult> AddFertilizer([FromBody] Fertilizer fertilizer)
        {
            if (fertilizer == null)
            {
                return BadRequest(new { message = "Fertilizer data is required." });
            }

            if (string.IsNullOrWhiteSpace(fertilizer.FertilizerType) || string.IsNullOrWhiteSpace(fertilizer.RecommendedAmount))
            {
                return BadRequest(new { message = "Fertilizer Type and Recommended Amount are required." });
            }

            // Ensure the referenced Crop exists
            var cropExists = await _context.Crops.AnyAsync(c => c.CropID == fertilizer.CropID);
            if (!cropExists)
            {
                return BadRequest(new { message = "Invalid CropID. The specified crop does not exist." });
            }

            try
            {
                _context.Fertilizers.Add(fertilizer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllFertilizers), new { id = fertilizer.FertilizerID }, fertilizer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the fertilizer.", error = ex.Message });
            }
        }

        // Delete a fertilizer by ID
        [HttpDelete("{FertilizerID}")]
        public async Task<IActionResult> DeleteFertilizer(int FertilizerID)
        {
            var fertilizer = await _context.Fertilizers.FindAsync(FertilizerID);
            if (fertilizer == null)
            {
                return NotFound(new { message = "Fertilizer not found." });
            }

            _context.Fertilizers.Remove(fertilizer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fertilizer deleted successfully." });
        }

        // Update fertilizer details
        [HttpPut("{FertilizerID}")]
        public async Task<IActionResult> UpdateFertilizer(int FertilizerID, [FromBody] Fertilizer fertilizer)
        {
            if (fertilizer == null)
            {
                return BadRequest(new { message = "Fertilizer data is required." });
            }

            if (string.IsNullOrWhiteSpace(fertilizer.FertilizerType) || string.IsNullOrWhiteSpace(fertilizer.RecommendedAmount))
            {
                return BadRequest(new { message = "Fertilizer Type and Recommended Amount are required." });
            }

            var existingFertilizer = await _context.Fertilizers.FindAsync(FertilizerID);
            if (existingFertilizer == null)
            {
                return NotFound(new { message = "Fertilizer not found." });
            }

            try
            {
                existingFertilizer.FertilizerType = fertilizer.FertilizerType;
                existingFertilizer.RecommendedAmount = fertilizer.RecommendedAmount;
                existingFertilizer.CropID = fertilizer.CropID;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Fertilizer updated successfully.", fertilizer = existingFertilizer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the fertilizer.", error = ex.Message });
            }
        }
    }
}
