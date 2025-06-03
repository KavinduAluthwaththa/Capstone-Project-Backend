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
                inputColumnNames: new[] { "float_input" },
                outputColumnNames: new[] { "output_label", "output_probability" } // this is fine
            );
            var model = pipeline.Fit(dataView);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<CropInput, CropPrediction>(model);
        }

        public CropPrediction Predict(CropInput input)
        {
            return _predictionEngine.Predict(input);
        }
    }
}