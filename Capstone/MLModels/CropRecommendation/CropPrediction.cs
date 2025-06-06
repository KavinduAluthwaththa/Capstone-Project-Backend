using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.CropRecommendation
{
    /// <summary>
    /// Represents the output from the ONNX crop recommendation model
    /// </summary>
    public class CropPrediction
    {
        /// <summary>
        /// The predicted crop label from the model
        /// </summary>
        [ColumnName("output_label")]
        public string[] output_label { get; set; } = Array.Empty<string>();
    }
}