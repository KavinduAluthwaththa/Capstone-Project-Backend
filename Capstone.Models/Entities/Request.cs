using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        //foreign key
        public int FarmerID { get; set; }
        //foreign key
        public int ShopID { get; set; }

        [ForeignKey("FarmerID")]
        public virtual Farmer Farmer { get; set; }

        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }


    }
}