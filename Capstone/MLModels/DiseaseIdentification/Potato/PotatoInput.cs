using Microsoft.ML.Data;
using System;

namespace Capstone.MLModels.DiseaseIdentification.Potato
{
    /// <summary>
    /// Input class for potato disease identification model
    /// </summary>
    public class PotatoInput
    {
        /// <summary>
        /// Input image tensor for the model
        /// Shape: [batch, height, width, channels] = [1, 256, 256, 3]
        /// </summary>
        [ColumnName("serving_default_input_layer_1:0")]
        [VectorType(1, 256, 256, 3)]
        public float[] ImageTensor { get; set; } = Array.Empty<float>();
    }
}
