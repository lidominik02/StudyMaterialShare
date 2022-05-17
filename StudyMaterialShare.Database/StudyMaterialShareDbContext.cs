using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.Database
{
    public class StudyMaterialShareDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<StudyMaterial> StudyMaterials { get; set; } = null!;

        public DbSet<Rating> Ratings { get; set; } = null!;

        public StudyMaterialShareDbContext() { }

        public StudyMaterialShareDbContext(DbContextOptions options): base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rating>()
                .HasIndex(r => new { r.StudyMaterialId,r.UserId })
                .IsUnique();
        }
    }
}
