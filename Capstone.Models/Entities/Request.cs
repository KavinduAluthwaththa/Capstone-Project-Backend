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
        public int FarmerID { get; set; }
        public int ShopID { get; set; }

        [ForeignKey("Farmer")]
        public virtual Farmer Farmer { get; set; }

        [ForeignKey("Shop")]
        public virtual Shop Shop { get; set; }


    }
}