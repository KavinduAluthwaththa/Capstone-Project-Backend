using Microsoft.AspNetCore.Mvc;
using Capstone.MLModels.DiseaseIdentification.Pumpkin;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace Capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PumpkinDiseaseController : ControllerBase
    {
        private readonly PumpkinModelHelper _modelHelper;
        private readonly ILogger<PumpkinDiseaseController> _logger;

        public PumpkinDiseaseController(PumpkinModelHelper modelHelper, ILogger<PumpkinDiseaseController> logger)
        {
            _modelHelper = modelHelper;
            _logger = logger;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Predict(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file type. Only JPG, PNG, and BMP files are allowed.");
                }

                _logger.LogInformation("Processing pumpkin disease prediction for file: {FileName}", imageFile.FileName);

                // Convert image to tensor
                var imageTensor = await ConvertImageToTensor(imageFile);
                
                var input = new PumpkinInput
                {
                    ImageTensor = imageTensor
                };

                var result = _modelHelper.Predict(input);

                _logger.LogInformation("Pumpkin disease prediction successful: {Disease} with {Confidence:P1} confidence", 
                    result.Disease, result.Confidence);

                return Ok(new
                {
                    disease = result.Disease,
                    confidence = result.Confidence,
                    probabilities = result.Probabilities
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pumpkin disease prediction failed");
                return StatusCode(500, $"Prediction failed: {ex.Message}");
            }
        }

        private async Task<float[]> ConvertImageToTensor(IFormFile imageFile)
        {
            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;

            using var image = new Bitmap(stream);
            using var resized = new Bitmap(image, new Size(256, 256));
            
            var tensor = new float[256 * 256 * 3];
            int index = 0;

            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    var pixel = resized.GetPixel(x, y);
                    
                    // Normalize to [0, 1] range
                    tensor[index++] = pixel.R / 255.0f;  // Red channel
                    tensor[index++] = pixel.G / 255.0f;  // Green channel  
                    tensor[index++] = pixel.B / 255.0f;  // Blue channel
                }
            }

            return tensor;
        }
    }
}
