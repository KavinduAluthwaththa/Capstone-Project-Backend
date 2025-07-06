using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Capstone.MLModels.DiseaseIdentification.Rice
{
    public class RiceModelHelper
    {
        private readonly PredictionEngine<RiceInput, RicePrediction> _predictionEngine;
        private readonly string[] _diseaseClasses;

        public RiceModelHelper()
        {
            try
            {
                Console.WriteLine("Initializing RiceModelHelper...");
                
                var mlContext = new MLContext();
                var dataView = mlContext.Data.LoadFromEnumerable(new List<RiceInput>());
                
                // Check if model file exists
                var modelPath = "MLModels/DiseaseIdentification/Rice/rice_model.onnx";
                var fullPath = Path.GetFullPath(modelPath);
                Console.WriteLine($"Looking for rice model at: {fullPath}");
                
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Rice ONNX model file not found at: {fullPath}");
                }
                
                Console.WriteLine("Rice model file found, creating pipeline...");
                
                var pipeline = mlContext.Transforms.ApplyOnnxModel(
                    modelFile: modelPath,
                    inputColumnNames: new[] { "serving_default_input_layer_1:0" },
                    outputColumnNames: new[] { "StatefulPartitionedCall_1:0" }
                );
                
                Console.WriteLine("Fitting rice model...");
                var model = pipeline.Fit(dataView);
                
                Console.WriteLine("Creating rice prediction engine...");
                _predictionEngine = mlContext.Model.CreatePredictionEngine<RiceInput, RicePrediction>(model);
                
                // Define rice disease classes based on your Python code
                _diseaseClasses = new string[]
                {
                    "Bacteria Blight",
                    "Brown Spot", 
                    "Healthy"
                };
                
                Console.WriteLine("RiceModelHelper initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing RiceModelHelper: {ex}");
                throw new InvalidOperationException($"Failed to initialize RiceModelHelper: {ex.Message}", ex);
            }
        }

        public RiceDiseaseResult Predict(RiceInput input)
        {
            try
            {
                Console.WriteLine("Processing rice disease prediction...");
                
                if (input?.ImageTensor == null || input.ImageTensor.Length == 0)
                {
                    throw new ArgumentException("Image tensor cannot be null or empty");
                }

                // Expected tensor size: 1 * 256 * 256 * 3 = 196,608
                int expectedSize = 256 * 256 * 3;
                if (input.ImageTensor.Length != expectedSize)
                {
                    throw new ArgumentException($"Image tensor must be exactly {expectedSize} elements (256x256x3), got {input.ImageTensor.Length}");
                }

                var prediction = _predictionEngine.Predict(input);
                
                if (prediction.Probabilities == null || prediction.Probabilities.Length != _diseaseClasses.Length)
                {
                    throw new InvalidOperationException("Invalid prediction output");
                }

                // Find the predicted class
                int predictedClassIndex = Array.IndexOf(prediction.Probabilities, prediction.Probabilities.Max());
                string predictedDisease = _diseaseClasses[predictedClassIndex];
                float confidence = prediction.Probabilities[predictedClassIndex];
                
                Console.WriteLine($"Predicted disease: {predictedDisease} with confidence: {confidence:P1}");
                
                // Create probability dictionary
                var probabilities = new Dictionary<string, float>();
                for (int i = 0; i < _diseaseClasses.Length; i++)
                {
                    probabilities[_diseaseClasses[i]] = prediction.Probabilities[i];
                }
                
                return new RiceDiseaseResult
                {
                    Disease = predictedDisease,
                    Confidence = confidence,
                    Probabilities = probabilities
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rice prediction error: {ex}");
                throw new InvalidOperationException($"Rice prediction failed: {ex.Message}", ex);
            }
        }
    }
    
    public class RiceDiseaseResult
    {
        public string Disease { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}
