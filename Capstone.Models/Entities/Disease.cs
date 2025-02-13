using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Entities
{
    public class Disease
    {
        [Key]
        public int DiseaseID { get; set; }
        public string DiseaseName { get; set; }
        public string Symptoms { get; set; }
        public string Treatement { get; set; }
    }
}