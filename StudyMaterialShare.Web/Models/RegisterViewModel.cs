using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudyMaterialShare.Web.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Felhasználó név megadása kötelező!")]
        [DisplayName("Felhasználónév")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Megjelenítendő név megadása kötelező!")]
        [DisplayName("Megjelenítendő név")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Jelszó megadása kötelező!")]
        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Jelszó megerősítésének megadása kötelező!")]
        [DisplayName("Jelszó megerősítése")]
        [Compare("Password", ErrorMessage = "Nem egyezik a két jelszó!")]
        [DataType(DataType.Password)]
        public string PasswordConfirmed { get; set; } = null!;

        [Required(ErrorMessage = "Email cím megadása kötelező!")]
        [DisplayName("Email cím")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
