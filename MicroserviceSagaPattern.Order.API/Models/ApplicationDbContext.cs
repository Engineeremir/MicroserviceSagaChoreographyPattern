using Microsoft.EntityFrameworkCore;

namespace MicroserviceSagaPattern.Order.API.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Order>Orders { get; set; }
        public DbSet<OrderItem>OrderItems { get; set; }

    }
}
