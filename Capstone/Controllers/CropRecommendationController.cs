using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Capstone.MLModels.CropRecommendation;

namespace Capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CropRecommendationController : ControllerBase
    {
        private readonly CropModelHelper _modelHelper;

        public CropRecommendationController(CropModelHelper modelHelper)
        {
            _modelHelper = modelHelper;
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] CropInput input)
        {
            var prediction = _modelHelper.Predict(input);
            return Ok(prediction.output.FirstOrDefault());
        }
    }
}