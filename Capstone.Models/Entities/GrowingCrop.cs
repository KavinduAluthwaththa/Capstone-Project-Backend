using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class GrowingCrop
    {
        [Key]
        public int cfid { get; set; }
        public int CropID { get; set; }
        public int FarmerID { get; set; }
        public string Date { get; set; }

        [ForeignKey("CropID")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("FarmerID")]
        public virtual Farmer Farmer { get; set; }
    }
}