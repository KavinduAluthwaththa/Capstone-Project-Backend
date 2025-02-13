using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class CropShop
    {
        [Key]
        public int CropID { get; set; }
        [Key]
        public int ShopID { get; set; }
        public string Date { get; set; }

        [ForeignKey("Crop")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("Shop")]
        public virtual Shop Shop { get; set; }
    }
}