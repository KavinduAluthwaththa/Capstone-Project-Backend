using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class MarketData
    {
        [Key]
        public int MarketDataID { get; set; }
        public int Price { get; set; }
        public string Date { get; set; }
    }
}