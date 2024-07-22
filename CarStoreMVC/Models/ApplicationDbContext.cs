using Microsoft.EntityFrameworkCore;

namespace CarStoreMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
    }
}
