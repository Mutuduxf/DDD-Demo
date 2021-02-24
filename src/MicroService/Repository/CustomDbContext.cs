using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Repository.EntityConfigurations;

namespace Repository
{
    public sealed class CustomDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}