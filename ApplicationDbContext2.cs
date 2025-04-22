using DbTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace DbTransfer
{
    public class ApplicationDbContext2 : DbContext
    {
        public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options) : base(options)
        {
        }
        // tables : 
        public DbSet<Car> Cars { get; set; }
    }
}
