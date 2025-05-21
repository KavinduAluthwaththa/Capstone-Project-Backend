using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class Fertilizer
    {
        [Key]
        public int FertilizerID { get; set; }
        //foreign key
        public int CropID { get; set; }
        public string FertilizerType { get; set; }
        //per hectare kg
        public string RecommendedAmount { get; set; }

        [ForeignKey("CropID")]
        public virtual Crop Crop { get; set; }

    }
}