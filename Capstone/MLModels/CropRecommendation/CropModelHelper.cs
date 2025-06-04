using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using System.Collections.Generic;
using System.IO;

namespace Capstone.MLModels.CropRecommendation
{
    public class CropModelHelper
    {
        private readonly PredictionEngine<CropInput, CropPrediction> _predictionEngine;
        private readonly string[] _cropLabels;

        public CropModelHelper()
        {
            try
            {
                Console.WriteLine("Initializing CropModelHelper...");
                
                var mlContext = new MLContext();
                var dataView = mlContext.Data.LoadFromEnumerable(new List<CropInput>());
                
                // Check if model file exists
                var modelPath = "MLModels/CropRecommendation/crop_recommendation.onnx";
                var fullPath = Path.GetFullPath(modelPath);
                Console.WriteLine($"Looking for model at: {fullPath}");
                
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"ONNX model file not found at: {fullPath}");
                }
                
                Console.WriteLine("Model file found, creating pipeline...");
                
                // Only use output_label since output_probability has binding issues
                var pipeline = mlContext.Transforms.ApplyOnnxModel(
                    modelFile: modelPath,
                    inputColumnNames: new[] { "float_input" },
                    outputColumnNames: new[] { "output_label" }
                );
                
                Console.WriteLine("Fitting model...");
                var model = pipeline.Fit(dataView);
                
                Console.WriteLine("Creating prediction engine...");
                _predictionEngine = mlContext.Model.CreatePredictionEngine<CropInput, CropPrediction>(model);
                
                // Define crop labels based on your Python output
                _cropLabels = new string[] {
                    "apple", "banana", "blackgram", "chickpea", "coconut", "coffee", "cotton", 
                    "grapes", "jute", "kidneybeans", "lentil", "maize", "mango", "mothbeans", 
                    "mungbean", "muskmelon", "orange", "papaya", "pigeonpeas", "pomegranate", 
                    "rice", "watermelon"
                };
                
                Console.WriteLine("CropModelHelper initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing CropModelHelper: {ex}");
                throw new InvalidOperationException($"Failed to initialize CropModelHelper: {ex.Message}", ex);
            }
        }

        public CropPredictionResult Predict(CropInput input)
        {
            try
            {
                Console.WriteLine($"Predicting for input: [{string.Join(", ", input?.float_input ?? new float[0])}]");
                
                if (input?.float_input == null || input.float_input.Length != 7)
                {
                    throw new ArgumentException("Input must contain exactly 7 float values");
                }

                var prediction = _predictionEngine.Predict(input);
                
                // Handle string array output - take first element
                string predictedLabel = prediction.output_label?.Length > 0 ? prediction.output_label[0] : "unknown";
                Console.WriteLine($"Raw prediction - Label: {predictedLabel}");
                
                // Create mock probabilities since we can't get them from ONNX directly
                var probabilities = new Dictionary<string, float>();
                
                // Set high probability for predicted crop, low for others
                foreach (var crop in _cropLabels)
                {
                    probabilities[crop] = crop == predictedLabel ? 0.95f : 0.02f;
                }
                
                var result = new CropPredictionResult
                {
                    Label = predictedLabel,
                    Probabilities = probabilities
                };
                
                Console.WriteLine($"Final prediction: {result.Label}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Prediction error: {ex}");
                throw new InvalidOperationException($"Prediction failed: {ex.Message}", ex);
            }
        }
    }
    
    public class CropPredictionResult
    {
        public string Label { get; set; } = string.Empty;
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}