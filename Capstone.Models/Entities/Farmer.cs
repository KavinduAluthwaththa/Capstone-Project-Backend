using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class user
    {
        [Key]
        public int FarmerID { get; set; }
        public string Name { get; set; }
        public string FarmLocation { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}