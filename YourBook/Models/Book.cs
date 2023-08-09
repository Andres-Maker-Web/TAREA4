using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourBook.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        [DisplayName("Titulo")]
        public string? Title { get; set; }
        public int Rating { get; set; }
        
    }
}
