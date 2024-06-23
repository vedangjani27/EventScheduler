using EventScheduler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Event> Events { get; set; }
    }
}
