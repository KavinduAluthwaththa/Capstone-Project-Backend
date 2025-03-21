using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Crop
    {
        [Key]
        public int CropID { get; set; }
        public string CropName { get; set; }
        public string PlantingSeason { get; set; }
    }
}