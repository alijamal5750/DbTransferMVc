using DbTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace DbTransfer
{
    public class ApplicationDbContext1 : DbContext
    {
        public ApplicationDbContext1(DbContextOptions<ApplicationDbContext1> options) : base(options)
        {
        }
        // tables 
        public DbSet<Car> cars { get; set; }
    }
}
