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
    public class CropDiseaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CropDiseaseController(AppDbContext context)
        {
            _context = context;
        }

        // Get all crop disease records
        [HttpGet]
        public async Task<IActionResult> GetAllCropDiseases()
        {
            var cropDisease = await _context.CropDiseases
                .Include(cd => cd.Crop)
                .Include(cd => cd.Disease)
                .Select(cd => new
                {
                    cd.cdid,
                    CropName = cd.Crop.CropName,
                    DiseaseName = cd.Disease.DiseaseName,
                    cd.Date
                })
                .ToListAsync();

            return Ok(cropDisease);
        }

        // Add a new crop disease record
        [HttpPost]
        public async Task<IActionResult> AddCropDisease([FromBody] CropDisease cropDisease)
        {
            if (cropDisease == null)
            {
                return BadRequest(new { message = "Crop disease data is required." });
            }

            if (string.IsNullOrWhiteSpace(cropDisease.Date))
            {
                return BadRequest(new { message = "Date is required." });
            }

            // Validate CropID and DiseaseID
            var cropExists = await _context.Crops.AnyAsync(c => c.CropID == cropDisease.CropID);
            var diseaseExists = await _context.Diseases.AnyAsync(d => d.DiseaseID == cropDisease.DiseaseID);

            if (!cropExists)
            {
                return BadRequest(new { message = "Invalid CropID. The specified crop does not exist." });
            }

            if (!diseaseExists)
            {
                return BadRequest(new { message = "Invalid DiseaseID. The specified disease does not exist." });
            }

            try
            {
                await _context.CropDiseases.AddAsync(cropDisease);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllCropDiseases), new { id = cropDisease.cdid }, cropDisease);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the crop disease record.", error = ex.Message });
            }
        }

        // Delete a crop disease record by ID
        [HttpDelete("{cdid}")]
        public async Task<IActionResult> DeleteCropDisease(int cdid)
        {
            var cropDisease = await _context.CropDiseases.FindAsync(cdid);
            if (cropDisease == null)
            {
                return NotFound(new { message = "Crop disease record not found." });
            }

            _context.CropDiseases.Remove(cropDisease);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Crop disease record deleted successfully." });
        }

        // Update a crop disease record
        [HttpPut("{cdid}")]
        public async Task<IActionResult> UpdateCropDisease(int cdid, [FromBody] CropDisease cropDisease)
        {
            if (cropDisease == null)
            {
                return BadRequest(new { message = "Crop disease data is required." });
            }

            if (string.IsNullOrWhiteSpace(cropDisease.Date))
            {
                return BadRequest(new { message = "Date is required." });
            }

            var existingCropDisease = await _context.CropDiseases.FindAsync(cdid);
            if (existingCropDisease == null)
            {
                return NotFound(new { message = "Crop disease record not found." });
            }

            // Validate CropID and DiseaseID
            var cropExists = await _context.Crops.AnyAsync(c => c.CropID == cropDisease.CropID);
            var diseaseExists = await _context.Diseases.AnyAsync(d => d.DiseaseID == cropDisease.DiseaseID);

            if (!cropExists)
            {
                return BadRequest(new { message = "Invalid CropID. The specified crop does not exist." });
            }

            if (!diseaseExists)
            {
                return BadRequest(new { message = "Invalid DiseaseID. The specified disease does not exist." });
            }

            try
            {
                existingCropDisease.CropID = cropDisease.CropID;
                existingCropDisease.DiseaseID = cropDisease.DiseaseID;
                existingCropDisease.Date = cropDisease.Date;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Crop disease record updated successfully.", cropDisease = existingCropDisease });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the crop disease record.", error = ex.Message });
            }
        }
    }
}


