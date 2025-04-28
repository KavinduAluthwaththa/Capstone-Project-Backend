using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Inspector
    {
        [Key]
        public int InspectorID { get; set; }
        public int PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
    }
}