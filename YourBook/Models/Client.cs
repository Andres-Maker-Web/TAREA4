using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourBook.Models
{
    public class Client: IdentityUser
    {
        [Key]
        public int ClientId { get; set; }
        [Required]
        [DisplayName("Nombre")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        public string? ClientName { get; set; }
        [Required]
        [DisplayName("Apellido")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        public string? ClientLastName { get; set; }
        [Required]
        [DisplayName("Correo")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string? Password { get; set; }
        [Required]
        [DisplayName("Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}
