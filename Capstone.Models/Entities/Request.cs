using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public string Date { get; set; }
        //in lkr
        public int Price { get; set; }
        //in kg
        public int Amount { get; set; }
        //foreign key
        public int CropID { get; set; }
        //foreign key
        public int ShopID { get; set; }

        // Availability section
        public bool IsAvailable { get; set; } 

        [ForeignKey("CropID")]
        public virtual Crop crop { get; set; }

        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }
    }
}