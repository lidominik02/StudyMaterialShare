using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.Web.Models
{
    public class StudyMaterialViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tananyag címe")]
        public string Title { get; set; } = null!;

        [DisplayName("Feltöltve")]
        public DateTime UploadedAt { get; set; }

        [DisplayName("Letöltések száma")]
        public int Downloads { get; set; }

        [DisplayName("Értéklések átlaga")]
        public double AvarageRating { get; set; }

        [Required]
        [DisplayName("Tantárgy neve")]
        public string Subject { get; set; } = null!;

        [DisplayName("Feltöltő neve")]
        public string? User { get; set; } = null!;

        [DisplayName("Tananyag")]
        [DataType(DataType.Upload)]
        public byte[]? File { get; set; } = null!;

        public StudyMaterialViewModel()
        {

        }

        public StudyMaterialViewModel(StudyMaterial sm)
        {
            Id = sm.Id;
            Title = sm.Title;
            Downloads = sm.Downloads;
            UploadedAt = sm.UploadedAt;
            Downloads = sm.Downloads;
            Subject = sm.Subject.Name;
            AvarageRating = sm.AvarageRating;
            User = sm.User.DisplayName;
        }
    }
}
