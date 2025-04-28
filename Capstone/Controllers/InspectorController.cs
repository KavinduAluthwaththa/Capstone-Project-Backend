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
    public class InspectorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InspectorController(AppDbContext context)
        {
            _context = context;
        }

        // Get all inspectors
        [HttpGet]
        public async Task<IActionResult> GetAllInspectors()
        {
            var inspectors = await _context.Inspectors.ToListAsync();
            return Ok(inspectors);
        }

        // Get an inspector by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspectorById(int id)
        {
            var inspector = await _context.Inspectors.FindAsync(id);
            if (inspector == null)
            {
                return NotFound(new { message = "Inspector not found." });
            }
            return Ok(inspector);
        }

        // Add a new inspector
        [HttpPost]
        public async Task<IActionResult> AddInspector([FromBody] Inspector inspector)
        {
            if (inspector == null)
            {
                return BadRequest(new { message = "Inspector data is required." });
            }

            if (string.IsNullOrWhiteSpace(inspector.Name) || string.IsNullOrWhiteSpace(inspector.Location))
            {
                return BadRequest(new { message = "Name and Designation are required." });
            }

            if (inspector.PhoneNumber.ToString().Length != 10)
            {
                return BadRequest(new { message = "PhoneNumber must be 10 digits." });
            }

            try
            {
                _context.Inspectors.Add(inspector);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInspectorById), new { id = inspector.InspectorID }, inspector);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the inspector.", error = ex.Message });
            }
        }

        // Update an inspector by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspector(int id, [FromBody] Inspector inspector)
        {
            if (inspector == null)
            {
                return BadRequest(new { message = "Inspector data is required." });
            }

            var existingInspector = await _context.Inspectors.FindAsync(id);
            if (existingInspector == null)
            {
                return NotFound(new { message = "Inspector not found." });
            }

            if (string.IsNullOrWhiteSpace(inspector.Name) || string.IsNullOrWhiteSpace(inspector.Location))
            {
                return BadRequest(new { message = "Name and Designation are required." });
            }

            if (inspector.PhoneNumber.ToString().Length != 10)
            {
                return BadRequest(new { message = "PhoneNumber must be 10 digits." });
            }

            try
            {
                existingInspector.Name = inspector.Name;
                existingInspector.Location = inspector.Location;
                existingInspector.PhoneNumber = inspector.PhoneNumber;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Inspector updated successfully.", inspector = existingInspector });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the inspector.", error = ex.Message });
            }
        }

        // Delete an inspector by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspector(int id)
        {
            var inspector = await _context.Inspectors.FindAsync(id);
            if (inspector == null)
            {
                return NotFound(new { message = "Inspector not found." });
            }

            _context.Inspectors.Remove(inspector);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Inspector deleted successfully." });
        }
    }
}
