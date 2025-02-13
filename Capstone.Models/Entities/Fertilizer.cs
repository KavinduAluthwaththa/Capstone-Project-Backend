using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class Fertilizer
    {
        [Key]
        public int FertilizerID { get; set; }
        public int CropID { get; set; }
        public string FertilizerType { get; set; }
        public string RecommendedAmount { get; set; }

        [ForeignKey("Crop")]
        public virtual Crop Crop { get; set; }

    }
}