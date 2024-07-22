using System.ComponentModel.DataAnnotations;

namespace CarStoreMVC.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Model { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Desc { get; set; } = string.Empty;
        public bool Availability { get; set; }
        [Required]
        public decimal Price { get; set; }
      
        public IFormFile? ImageFile { get; set; } 
    }
}
