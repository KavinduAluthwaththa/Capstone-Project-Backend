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
    public class DiseaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiseaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Disease
        [HttpGet]
        public async Task<IActionResult> GetAllDiseases()
        {
            var diseases = await _context.Diseases.ToListAsync();
            return Ok(diseases);
        }

        // GET: api/Disease/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);

            if (disease == null)
            {
                return NotFound(new { message = "Disease not found" });
            }

            return Ok(disease);
        }

        // POST: api/Disease
        [HttpPost]
        public async Task<IActionResult> AddDisease([FromBody] Disease disease)
        {
            if (disease == null)
            {
                return BadRequest(new { message = "Disease data is required." });
            }

            if (string.IsNullOrWhiteSpace(disease.DiseaseName) ||
                string.IsNullOrWhiteSpace(disease.Symptoms) ||
                string.IsNullOrWhiteSpace(disease.Treatment))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            try
            {
                _context.Diseases.Add(disease);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDisease), new { id = disease.DiseaseID }, disease);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the disease.", error = ex.Message });
            }
        }

        // PUT: api/Disease/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisease(int id, [FromBody] Disease disease)
        {
            if (disease == null)
            {
                return BadRequest(new { message = "Disease data is required." });
            }

            var existingDisease = await _context.Diseases.FindAsync(id);
            if (existingDisease == null)
            {
                return NotFound(new { message = "Disease not found." });
            }

            try
            {
                existingDisease.DiseaseName = disease.DiseaseName;
                existingDisease.Symptoms = disease.Symptoms;
                existingDisease.Treatment = disease.Treatment;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Disease updated successfully.", disease = existingDisease });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the disease.", error = ex.Message });
            }
        }

        // DELETE: api/Disease/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null)
            {
                return NotFound(new { message = "Disease not found" });
            }

            try
            {
                _context.Diseases.Remove(disease);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Disease deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the disease.", error = ex.Message });
            }
        }
    }
}
