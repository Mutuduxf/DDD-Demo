using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;

namespace Repository.DbContext
{
    public sealed class CustomDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> Users { get; set; }

        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.BuildUser();
        }
    }
}