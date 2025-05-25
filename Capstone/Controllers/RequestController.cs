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
        public async Task<IActionResult> GetAllAvailableRequests()
        {
            var requests = await _context.Requests
                .Where(r => r.IsAvailable == true)  // Only get available requests
                .Include(r => r.Farmer)
                .Include(r => r.Shop)
                .ToListAsync();
            return Ok(requests);
        }

        // GET: api/request/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvailableRequestById(int id)
        {
            var request = await _context.Requests
                .Where(r => r.IsAvailable == true)  // Only check available requests
                .Include(r => r.Farmer)
                .Include(r => r.Shop)
                .FirstOrDefaultAsync(r => r.RequestID == id);

            if (request == null)
            {
                return NotFound(new { message = "Request not found or not available" });
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

            // Set new requests as available by default
            request.IsAvailable = true;

            try
            {
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAvailableRequestById),
                    new { id = request.RequestID }, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the request.",
                    error = ex.Message
                });
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

            var existingRequest = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestID == id && r.IsAvailable == true);

            if (existingRequest == null)
            {
                return NotFound(new { message = "Request not found or not available" });
            }

            try
            {
                existingRequest.Date = request.Date;
                existingRequest.Amount = request.Amount;
                existingRequest.FarmerID = request.FarmerID;
                existingRequest.ShopID = request.ShopID;
                existingRequest.IsAvailable = request.IsAvailable; // Allow updating availability

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Request updated successfully.",
                    request = existingRequest
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the request.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/request/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestID == id && r.IsAvailable == true);

            if (request == null)
            {
                return NotFound(new { message = "Request not found or not available" });
            }

            try
            {
                // Soft delete by setting IsAvailable to false
                request.IsAvailable = false;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Request marked as unavailable successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the request.",
                    error = ex.Message
                });
            }
        }
    }
}