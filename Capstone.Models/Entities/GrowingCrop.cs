using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class GrowingCrop
    {
        [Key, Column(Order = 0)]
        public int CropID { get; set; }
        [Key, Column(Order = 1)]
        public int FarmerID { get; set; }
        public string Date { get; set; }

        [ForeignKey("Crop")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("Farmer")]
        public virtual Farmer Farmer { get; set; }
    }
}