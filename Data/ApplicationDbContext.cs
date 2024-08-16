using Microsoft.EntityFrameworkCore;
using Cater4Us_Backend.Models.Entities;

namespace Cater4Us_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> FoodModel { get; set; }
        public DbSet<Users> Users { get; set; } // Added Users DbSet


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration (if any) can go here

            // Example: Setting up unique constraint for ApiKey

            // Other configurations...
        }
    }
}
