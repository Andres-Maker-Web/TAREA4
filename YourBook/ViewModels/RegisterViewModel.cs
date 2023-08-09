using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourBook.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Nombre usuario")]
        [StringLength(10)]
        public string Username { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
