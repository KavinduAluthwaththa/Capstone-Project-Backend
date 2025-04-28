using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Shop
    {
        [Key]
        public int ShopID { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Location { get; set; }
    }
}