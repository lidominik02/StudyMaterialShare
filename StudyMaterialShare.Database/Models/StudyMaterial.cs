using System.ComponentModel.DataAnnotations;

namespace StudyMaterialShare.Database.Models
{
    public class StudyMaterial
    {
        //tantárgy, letöltések száma, feltöltő felhasználó

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public byte[] File { get; set; } = null!;

        public string FileType { get; set; } = null!;

        public DateTime UploadedAt { get; set; }

        public int Downloads { get; set; }

        public virtual Subject Subject { get; set; } = null!;

        public virtual ICollection<Rating> Ratings { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public double AvarageRating
        {
            get {
                try
                {
                    double avg = this.Ratings
                        .Where(rating => rating.StudyMaterial.Id == this.Id)
                        .Select(rating => rating.RateValue)
                        .Average();

                    return Math.Round(avg, 2);

                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}
