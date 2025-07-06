using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.DiseaseIdentification.Pumpkin
{
    /// <summary>
    /// Represents the output from the ONNX pumpkin disease identification model
    /// </summary>
    public class PumpkinPrediction
    {
        /// <summary>
        /// The predicted disease probabilities from the model
        /// Shape: [batch, num_classes] = [1, 5]
        /// </summary>
        [ColumnName("StatefulPartitionedCall_1:0")]
        public float[] Probabilities { get; set; } = Array.Empty<float>();
    }
}
