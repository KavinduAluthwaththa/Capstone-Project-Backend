using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.DiseaseIdentification.Rice
{
    /// <summary>
    /// Represents the output from the ONNX rice disease identification model
    /// </summary>
    public class RicePrediction
    {
        /// <summary>
        /// The predicted disease probabilities from the model
        /// Shape: [batch, num_classes] = [1, 3]
        /// </summary>
        [ColumnName("StatefulPartitionedCall_1:0")]
        public float[] Probabilities { get; set; } = Array.Empty<float>();
    }
}
