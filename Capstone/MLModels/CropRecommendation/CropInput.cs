using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.CropRecommendation
{
    /// <summary>
    /// Input class for crop recommendation model
    /// </summary>
    public class CropInput
    {
        // Individual properties for better clarity
        [ColumnName("N")]
        public float Nitrogen { get; set; }  // Nitrogen content in soil (ratio)

        [ColumnName("P")]
        public float Phosphorus { get; set; }  // Phosphorus content in soil (ratio)

        [ColumnName("K")] 
        public float Potassium { get; set; }  // Potassium content in soil (ratio)

        [ColumnName("temperature")]
        public float Temperature { get; set; }  // Temperature in celsius

        [ColumnName("humidity")]
        public float Humidity { get; set; }  // Relative humidity in %

        [ColumnName("ph")]
        public float Ph { get; set; }  // pH value of the soil

        [ColumnName("rainfall")]
        public float Rainfall { get; set; }  // Rainfall in mm

        // Vector representation needed for ML.NET model input
        [ColumnName("float_input")]
        [VectorType(7)]
        public float[] FloatInput => new float[]
        {
            Nitrogen,
            Phosphorus,
            Potassium,
            Temperature,
            Humidity,
            Ph,
            Rainfall
        };
    }
}