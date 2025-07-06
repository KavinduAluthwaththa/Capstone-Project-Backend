using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Capstone.MLModels.DiseaseIdentification.Potato
{
    public class PotatoModelHelper
    {
        private readonly PredictionEngine<PotatoInput, PotatoPrediction> _predictionEngine;
        private readonly string[] _diseaseClasses;

        public PotatoModelHelper()
        {
            try
            {
                Console.WriteLine("Initializing PotatoModelHelper...");
                
                var mlContext = new MLContext();
                var dataView = mlContext.Data.LoadFromEnumerable(new List<PotatoInput>());
                
                // Check if model file exists
                var modelPath = "MLModels/DiseaseIdentification/Potato/potato_model.onnx";
                var fullPath = Path.GetFullPath(modelPath);
                Console.WriteLine($"Looking for potato model at: {fullPath}");
                
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Potato ONNX model file not found at: {fullPath}");
                }
                
                Console.WriteLine("Potato model file found, creating pipeline...");
                
                var pipeline = mlContext.Transforms.ApplyOnnxModel(
                    modelFile: modelPath,
                    inputColumnNames: new[] { "serving_default_input_layer_1:0" },
                    outputColumnNames: new[] { "StatefulPartitionedCall_1:0" }
                );
                
                Console.WriteLine("Fitting potato model...");
                var model = pipeline.Fit(dataView);
                
                Console.WriteLine("Creating potato prediction engine...");
                _predictionEngine = mlContext.Model.CreatePredictionEngine<PotatoInput, PotatoPrediction>(model);
                
                // Define potato disease classes based on your Python code
                _diseaseClasses = new string[]
                {
                    "Early Blight",
                    "Late Blight", 
                    "Healthy"
                };
                
                Console.WriteLine("PotatoModelHelper initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing PotatoModelHelper: {ex}");
                throw new InvalidOperationException($"Failed to initialize PotatoModelHelper: {ex.Message}", ex);
            }
        }

        public PotatoDiseaseResult Predict(PotatoInput input)
        {
            try
            {
                Console.WriteLine("Processing potato disease prediction...");
                
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
                
                return new PotatoDiseaseResult
                {
                    Disease = predictedDisease,
                    Confidence = confidence,
                    Probabilities = probabilities
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Potato prediction error: {ex}");
                throw new InvalidOperationException($"Potato prediction failed: {ex.Message}", ex);
            }
        }
    }
    
    public class PotatoDiseaseResult
    {
        public string Disease { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}
