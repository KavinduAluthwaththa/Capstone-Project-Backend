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
            if (prediction.output_label == null || prediction.output_label.Length == 0)
                return BadRequest("No crop could be predicted for the given input.");
            // Only return label, since output_probability cannot be mapped by ML.NET
            var label = prediction.output_label[0];
            return Ok(new { label });
        }
    }
}