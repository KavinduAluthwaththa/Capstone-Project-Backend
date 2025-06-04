using Microsoft.ML.Data;

namespace Capstone.MLModels.CropRecommendation
{
    public class CropInput
    {
        [ColumnName("float_input")]
        [VectorType(7)]
        public float[] float_input { get; set; }
    }
}