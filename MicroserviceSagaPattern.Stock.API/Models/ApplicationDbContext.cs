using Microsoft.EntityFrameworkCore;

namespace MicroserviceSagaChoreographyPattern.Stock.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Stock> Stocks { get; set; }



    }
}
