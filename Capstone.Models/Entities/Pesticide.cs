using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Pesticide
    {
        [Key]
        public int PesticideID { get; set; }
        public int CropID { get; set; }
        public string PesticideType { get; set; }
        public int RecommendedAmount { get; set; }
    }
}