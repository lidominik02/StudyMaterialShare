using System.ComponentModel.DataAnnotations;

namespace StudyMaterialShare.Database.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        //public virtual ICollection<StudyMaterial> StudyMaterials { get; set; } = null!;
    }
}
