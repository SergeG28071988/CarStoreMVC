using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarStoreMVC.Models
{
    [Table("Cars")]
    public class Car
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Desc { get; set; } = string.Empty;
        public bool Availability { get; set; }
        [Precision(16, 2)]
        public decimal Price { get; set; }
        [MaxLength(100)]
        public string Image { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 
    }
}
