using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CustomDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User

            var userBuilder = modelBuilder.Entity<User>();
            userBuilder.ToTable("User")
                .HasKey(user => user.Id);

            userBuilder.Ignore(user => user.DomainEvents);

            userBuilder.Property(user => user.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();

            userBuilder.Property(user => user.Age)
                .HasColumnType("integer")
                .IsRequired();

            #endregion
        }
    }
}