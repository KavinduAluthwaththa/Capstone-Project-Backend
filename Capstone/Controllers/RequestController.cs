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
    public class RequestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/request
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _context.Requests
                .Include(r => r.Farmer)
                .Include(r => r.Shop)
                .ToListAsync();
            return Ok(requests);
        }

        // GET: api/request/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Farmer)
                .Include(r => r.Shop)
                .FirstOrDefaultAsync(r => r.RequestID == id);

            if (request == null)
            {
                return NotFound(new { message = "Request not found" });
            }

            return Ok(request);
        }

        // POST: api/request
        [HttpPost]
        public async Task<IActionResult> AddRequest([FromBody] Request request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request data is required." });
            }

            try
            {
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequestById), new { id = request.RequestID }, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the request.", error = ex.Message });
            }
        }

        // PUT: api/request/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] Request request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request data is required." });
            }

            var existingRequest = await _context.Requests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound(new { message = "Request not found" });
            }

            try
            {
                existingRequest.Date = request.Date;
                existingRequest.Amount = request.Amount;
                existingRequest.FarmerID = request.FarmerID;
                existingRequest.ShopID = request.ShopID;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Request updated successfully.", request = existingRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the request.", error = ex.Message });
            }
        }

        // DELETE: api/request/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound(new { message = "Request not found" });
            }

            try
            {
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Request deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the request.", error = ex.Message });
            }
        }
    }
}
