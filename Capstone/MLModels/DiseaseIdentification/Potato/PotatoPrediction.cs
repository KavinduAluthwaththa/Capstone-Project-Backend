using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.DiseaseIdentification.Potato
{
    /// <summary>
    /// Represents the output from the ONNX potato disease identification model
    /// </summary>
    public class PotatoPrediction
    {
        /// <summary>
        /// The predicted disease probabilities from the model
        /// Shape: [batch, num_classes] = [1, 3]
        /// </summary>
        [ColumnName("StatefulPartitionedCall_1:0")]
        public float[] Probabilities { get; set; } = Array.Empty<float>();
    }
}
