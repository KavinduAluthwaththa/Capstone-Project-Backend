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
    public class FarmerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FarmerController(AppDbContext context)
        {
            _context = context;
        }

        // Get all farmers
        [HttpGet]
        public async Task<IActionResult> GetAllFarmers()
        {
            var farmers = await _context.Farmers
                .Select(f => new SelectListItem
                {
                    Value = f.FarmerID.ToString(),
                    Text = f.Name
                })
                .ToListAsync();

            return Ok(farmers);
        }

        // Add a new farmer
        [HttpPost]
        public async Task<IActionResult> AddFarmer([FromBody] Farmer farmer)
        {
            if (farmer == null)
            {
                return BadRequest(new { message = "Farmer data is required." });
            }

            if (string.IsNullOrWhiteSpace(farmer.Name) || string.IsNullOrWhiteSpace(farmer.FarmLocation) || farmer.PhoneNumber <= 0)
            {
                return BadRequest(new { message = "Name, Farm Location, and valid Phone Number are required." });
            }

            try
            {
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllFarmers), new { id = farmer.FarmerID }, farmer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the farmer.", error = ex.Message });
            }
        }

        // Delete a farmer by ID
        [HttpDelete("{FarmerID}")]
        public async Task<IActionResult> DeleteFarmer(int FarmerID)
        {
            var farmer = await _context.Farmers.FindAsync(FarmerID);
            if (farmer == null)
            {
                return NotFound(new { message = "Farmer not found." });
            }

            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Farmer deleted successfully." });
        }

        // Update farmer details
        [HttpPut("{FarmerID}")]
        public async Task<IActionResult> UpdateFarmer(int FarmerID, [FromBody] Farmer farmer)
        {
            if (farmer == null)
            {
                return BadRequest(new { message = "Farmer data is required." });
            }

            if (string.IsNullOrWhiteSpace(farmer.Name) || string.IsNullOrWhiteSpace(farmer.FarmLocation) || farmer.PhoneNumber <= 0)
            {
                return BadRequest(new { message = "Name, Farm Location, and valid Phone Number are required." });
            }

            var existingFarmer = await _context.Farmers.FindAsync(FarmerID);
            if (existingFarmer == null)
            {
                return NotFound(new { message = "Farmer not found." });
            }

            try
            {
                existingFarmer.Name = farmer.Name;
                existingFarmer.FarmLocation = farmer.FarmLocation;
                existingFarmer.PhoneNumber = farmer.PhoneNumber;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Farmer updated successfully.", farmer = existingFarmer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the farmer.", error = ex.Message });
            }
        }
    }
}
