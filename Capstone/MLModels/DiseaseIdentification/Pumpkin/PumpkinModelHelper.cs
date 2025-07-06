using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Capstone.MLModels.DiseaseIdentification.Pumpkin
{
    public class PumpkinModelHelper
    {
        private readonly PredictionEngine<PumpkinInput, PumpkinPrediction> _predictionEngine;
        private readonly string[] _diseaseClasses;

        public PumpkinModelHelper()
        {
            try
            {
                Console.WriteLine("Initializing PumpkinModelHelper...");
                
                var mlContext = new MLContext();
                var dataView = mlContext.Data.LoadFromEnumerable(new List<PumpkinInput>());
                
                // Check if model file exists
                var modelPath = "MLModels/DiseaseIdentification/Pumpkin/pumpkin_model.onnx";
                var fullPath = Path.GetFullPath(modelPath);
                Console.WriteLine($"Looking for pumpkin model at: {fullPath}");
                
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Pumpkin ONNX model file not found at: {fullPath}");
                }
                
                Console.WriteLine("Pumpkin model file found, creating pipeline...");
                
                var pipeline = mlContext.Transforms.ApplyOnnxModel(
                    modelFile: modelPath,
                    inputColumnNames: new[] { "serving_default_input_layer_1:0" },
                    outputColumnNames: new[] { "StatefulPartitionedCall_1:0" }
                );
                
                Console.WriteLine("Fitting pumpkin model...");
                var model = pipeline.Fit(dataView);
                
                Console.WriteLine("Creating pumpkin prediction engine...");
                _predictionEngine = mlContext.Model.CreatePredictionEngine<PumpkinInput, PumpkinPrediction>(model);
                
                // Define pumpkin disease classes based on your Python code
                _diseaseClasses = new string[]
                {
                    "Downy Mildew",
                    "Bacterial Spot",
                    "Healthy",
                    "Powdery Mildew",
                    "Cucurbits"
                };
                
                Console.WriteLine("PumpkinModelHelper initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing PumpkinModelHelper: {ex}");
                throw new InvalidOperationException($"Failed to initialize PumpkinModelHelper: {ex.Message}", ex);
            }
        }

        public PumpkinDiseaseResult Predict(PumpkinInput input)
        {
            try
            {
                Console.WriteLine("Processing pumpkin disease prediction...");
                
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
                
                return new PumpkinDiseaseResult
                {
                    Disease = predictedDisease,
                    Confidence = confidence,
                    Probabilities = probabilities
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pumpkin prediction error: {ex}");
                throw new InvalidOperationException($"Pumpkin prediction failed: {ex.Message}", ex);
            }
        }
    }
    
    public class PumpkinDiseaseResult
    {
        public string Disease { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}
