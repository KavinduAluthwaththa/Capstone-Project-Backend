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
    public class CropController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CropController(AppDbContext context)
        {
            _context = context;
        }
        //get all crop details
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCrops()
        {
            var crops = await _context.Crops
                .Select(c => new SelectListItem
                {
                    Value = c.CropID.ToString(),
                    Text = c.CropName
                })
                .ToListAsync();

            return Ok(crops);

        }

        //add crops
        [HttpPost]
        public async Task<IActionResult> AddCrop([FromBody] Crop crop)
        {
            // Validate the crop object
            if (crop == null)
            {
                return BadRequest(new { message = "Crop data is required." });
            }

            if (string.IsNullOrWhiteSpace(crop.CropName) || string.IsNullOrWhiteSpace(crop.PlantingSeason))
            {
                return BadRequest(new { message = "Crop name and planting season are required." });
            }

            try
            {
                // Add the new crop to the database
                _context.Crops.Add(crop);
                await _context.SaveChangesAsync();

                // Return a successful response with the added crop
                return CreatedAtAction(nameof(GetAllCrops), new { id = crop.CropID }, crop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the crop.", error = ex.Message });
            }
        }


        // DELETE: api/crops/{id}
        [HttpDelete("{CropId}")]
        public async Task<IActionResult> DeleteCrop(int CropId)
        {
            var crop = await _context.Crops.FindAsync(CropId);
            if (crop == null)
            {
                return NotFound(new { message = "Crop not found" });
            }

            _context.Crops.Remove(crop);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Crop deleted successfully" });
        }


        //modify crop details
        [HttpPut("{CropId}")]
        public async Task<IActionResult> UpdateCrop(int CropId, [FromBody] Crop crop)
        {
            // Validate request body
            if (crop == null)
            {
                return BadRequest(new { message = "Crop data is required." });
            }

            if (string.IsNullOrWhiteSpace(crop.CropName) || string.IsNullOrWhiteSpace(crop.PlantingSeason))
            {
                return BadRequest(new { message = "Crop name and planting season are required." });
            }

            // Find the existing crop
            var existingCrop = await _context.Crops.FindAsync(CropId);
            if (existingCrop == null)
            {
                return NotFound(new { message = "Crop not found." });
            }

            try
            {
                // Update the crop properties
                existingCrop.CropName = crop.CropName;
                existingCrop.PlantingSeason = crop.PlantingSeason;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { message = "Crop updated successfully.", crop = existingCrop });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the crop.", error = ex.Message });
            }
        }


    }
}
