namespace Capstone.MLModels.CropRecommendation
{
    public class CropInput
    {
        public float N { get; set; }
        public float P { get; set; }
        public float K { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
        public float ph { get; set; }
        public float rainfall { get; set; }
    }
}