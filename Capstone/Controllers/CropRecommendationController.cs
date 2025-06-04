using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Capstone.MLModels.CropRecommendation;
using Microsoft.Extensions.Logging;

namespace Capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CropRecommendationController : ControllerBase
    {
        private readonly CropModelHelper _modelHelper;
        private readonly ILogger<CropRecommendationController> _logger;

        public CropRecommendationController(CropModelHelper modelHelper, ILogger<CropRecommendationController> logger)
        {
            _modelHelper = modelHelper;
            _logger = logger;
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] CropInput input)
        {
            try
            {
                _logger.LogInformation("Received prediction request with input: {Input}", 
                    input?.float_input != null ? string.Join(", ", input.float_input) : "null");

                if (input?.float_input == null)
                {
                    return BadRequest("Input cannot be null");
                }

                if (input.float_input.Length != 7)
                {
                    return BadRequest($"Expected 7 input values, got {input.float_input.Length}");
                }

                var result = _modelHelper.Predict(input);
                
                if (string.IsNullOrEmpty(result.Label))
                    return BadRequest("No crop could be predicted for the given input.");

                _logger.LogInformation("Prediction successful: {Label}", result.Label);

                return Ok(new
                {
                    label = result.Label,
                    probabilities = result.Probabilities
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Prediction failed");
                return StatusCode(500, $"Prediction failed: {ex.Message}");
            }
        }
    }
}