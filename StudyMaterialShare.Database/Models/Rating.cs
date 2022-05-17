using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMaterialShare.Database.Models
{
    public class Rating
    {
        //  felhasználó

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int RateValue { get; set; }

        [ForeignKey("StudyMaterial")]
        public int StudyMaterialId { get; set; }

        public virtual StudyMaterial StudyMaterial { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
