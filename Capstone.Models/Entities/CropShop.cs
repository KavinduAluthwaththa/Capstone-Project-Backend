using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class CropShop
    {
        [Key]
        public int csid { get; set; }
        public int CropID { get; set; }
        public int ShopID { get; set; }
        public string Date { get; set; }

        [ForeignKey("CropID")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }
    }
}