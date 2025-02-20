using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}