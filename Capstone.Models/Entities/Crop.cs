using System.ComponentModel.DataAnnotations;
using Capstone.Shared.Enums;

namespace Capstone.Models.Entities
{
    public class Crop
    {
        [Key]
        public int CropID { get; set; }
        public string CropName { get; set; }
        public Seasons PlantingSeason { get; set; }
    }
}