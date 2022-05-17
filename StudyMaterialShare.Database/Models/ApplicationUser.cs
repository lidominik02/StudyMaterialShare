using Microsoft.AspNetCore.Identity;

namespace StudyMaterialShare.Database.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string DisplayName { get; set; } = null!;
    }
}
