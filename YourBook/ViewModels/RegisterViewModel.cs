using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourBook.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DisplayName("Nombre usuario")]
        [StringLength(10)]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [StringLength(15, ErrorMessage = "Problem with Password", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
