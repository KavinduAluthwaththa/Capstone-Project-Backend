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
    public class PesticideController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PesticideController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/pesticide
        [HttpGet]
        public async Task<IActionResult> GetAllPesticides()
        {
            var pesticides = await _context.Pesticides.ToListAsync();
            return Ok(pesticides);
        }

        // GET: api/pesticide/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPesticideById(int id)
        {
            var pesticide = await _context.Pesticides.FindAsync(id);

            if (pesticide == null)
            {
                return NotFound(new { message = "Pesticide not found" });
            }

            return Ok(pesticide);
        }

        // POST: api/pesticide
        [HttpPost]
        public async Task<IActionResult> AddPesticide([FromBody] Pesticide pesticide)
        {
            if (pesticide == null)
            {
                return BadRequest(new { message = "Pesticide data is required." });
            }

            try
            {
                _context.Pesticides.Add(pesticide);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPesticideById), new { id = pesticide.PesticideID }, pesticide);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the pesticide.", error = ex.Message });
            }
        }

        // PUT: api/pesticide/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePesticide(int id, [FromBody] Pesticide pesticide)
        {
            if (pesticide == null)
            {
                return BadRequest(new { message = "Pesticide data is required." });
            }

            var existingPesticide = await _context.Pesticides.FindAsync(id);
            if (existingPesticide == null)
            {
                return NotFound(new { message = "Pesticide not found" });
            }

            try
            {
                existingPesticide.PesticideType = pesticide.PesticideType;
                existingPesticide.RecommendedAmount = pesticide.RecommendedAmount;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Pesticide updated successfully.", pesticide = existingPesticide });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the pesticide.", error = ex.Message });
            }
        }

        // DELETE: api/pesticide/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePesticide(int id)
        {
            var pesticide = await _context.Pesticides.FindAsync(id);

            if (pesticide == null)
            {
                return NotFound(new { message = "Pesticide not found" });
            }

            try
            {
                _context.Pesticides.Remove(pesticide);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Pesticide deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the pesticide.", error = ex.Message });
            }
        }
    }
}
