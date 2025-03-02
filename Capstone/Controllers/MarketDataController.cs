using Capstone.Models.Entities;
using Capstone.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarketDataController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/marketdata
        [HttpGet]
        public async Task<IActionResult> GetAllMarketData()
        {
            var marketData = await _context.MarketDatas.ToListAsync();
            return Ok(marketData);
        }

        // GET: api/marketdata/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketDataById(int id)
        {
            var marketData = await _context.MarketDatas.FindAsync(id);

            if (marketData == null)
            {
                return NotFound(new { message = "Market data not found" });
            }

            return Ok(marketData);
        }

        // POST: api/marketdata
        [HttpPost]
        public async Task<IActionResult> AddMarketData([FromBody] MarketData marketData)
        {
            if (marketData == null)
            {
                return BadRequest(new { message = "Market data is required." });
            }

            try
            {
                _context.MarketDatas.Add(marketData);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMarketDataById), new { id = marketData.MarketDataID }, marketData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the market data.", error = ex.Message });
            }
        }

        // PUT: api/marketdata/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarketData(int id, [FromBody] MarketData marketData)
        {
            if (marketData == null)
            {
                return BadRequest(new { message = "Market data is required." });
            }

            var existingMarketData = await _context.MarketDatas.FindAsync(id);
            if (existingMarketData == null)
            {
                return NotFound(new { message = "Market data not found" });
            }

            try
            {
                existingMarketData.Price = marketData.Price;
                existingMarketData.Date = marketData.Date;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Market data updated successfully.", marketData = existingMarketData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the market data.", error = ex.Message });
            }
        }

        // DELETE: api/marketdata/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarketData(int id)
        {
            var marketData = await _context.MarketDatas.FindAsync(id);

            if (marketData == null)
            {
                return NotFound(new { message = "Market data not found" });
            }

            try
            {
                _context.MarketDatas.Remove(marketData);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Market data deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the market data.", error = ex.Message });
            }
        }
    }
}
