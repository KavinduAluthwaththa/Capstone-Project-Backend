using Microsoft.ML.Data;

namespace Capstone.MLModels.CropRecommendation
{
    public class CropPrediction
    {
        [ColumnName("output_label")]
        public string[] output_label { get; set; } = new string[0];
    }
}