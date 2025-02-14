using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    public class CropDisease
    {
        [Key]
        public int cdid { get; set; }
        public int CropID { get; set; }
        public int DiseaseID { get; set; }
        public string Date { get; set; }

        [ForeignKey("CropID")]
        public virtual Crop Crop { get; set; }

        [ForeignKey("DiseaseID")]
        public virtual Disease Disease { get; set; }
    }
}