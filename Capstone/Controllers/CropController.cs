using Capstone.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        //get all crop details
        [HttpGet]
        public async Task<IActionResult> GetAllCrops(AppDbContext DbContext)
        {
            var crops = await DbContext.Crop
                .Select(c => new SelectListItem
                {
                    Value = c.CropID.ToString(),
                    Text = c.CropName
                })
                .ToListAsync();

            return Ok(crops);

        }

    }
}
