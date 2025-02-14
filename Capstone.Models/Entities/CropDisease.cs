using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class CropDisease
    {
        [Key, Column(Order =0)]
        public int CropID { get; set; }
        [Key, Column(Order = 1)]
        public int DiseaseID { get; set; }
        public string Date { get; set; }

        [ForeignKey("Crop")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("Disease")]
        public virtual Disease Disease { get; set; }
    }
}