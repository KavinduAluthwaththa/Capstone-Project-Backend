using Microsoft.ML;
using Microsoft.ML.Transforms.Onnx;
using System.Collections.Generic;

namespace Capstone.MLModels.CropRecommendation
{
    public class CropModelHelper
    {
        private readonly PredictionEngine<CropInput, CropPrediction> _predictionEngine;

        public CropModelHelper()
        {
            var mlContext = new MLContext();
            var dataView = mlContext.Data.LoadFromEnumerable(new List<CropInput>());
            var pipeline = mlContext.Transforms.ApplyOnnxModel(
                modelFile: "MLModels/CropRecommendation/crop_recommendation.onnx",
                inputColumnNames: new[] { "N", "P", "K", "temperature", "humidity", "ph", "rainfall" },
                outputColumnNames: new[] { "output" });
            var model = pipeline.Fit(dataView);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<CropInput, CropPrediction>(model);
        }

        public CropPrediction Predict(CropInput input)
        {
            return _predictionEngine.Predict(input);
        }
    }
}