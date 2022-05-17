using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudyMaterialShare.Web.Models
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Felhasználó név megadása kötelező!")]
        [DisplayName("Felhasználónév")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Jelszó megadása kötelező!")]
        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
