using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class CropShop
    {
        [Key, Column(Order = 0)]
        public int CropID { get; set; }
        [Key, Column(Order = 1)]
        public int ShopID { get; set; }
        public string Date { get; set; }

        [ForeignKey("Crop")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("Shop")]
        public virtual Shop Shop { get; set; }
    }
}