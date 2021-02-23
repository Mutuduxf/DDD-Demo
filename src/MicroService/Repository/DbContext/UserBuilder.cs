using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;

namespace Repository.DbContext
{
    public static class UserBuilder
    {
        public static ModelBuilder BuildUser(this ModelBuilder modelBuilder)
        {
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

            userBuilder.Property(user => user.Gender)
                .HasColumnType("integer")
                .IsRequired();

            userBuilder.OwnsOne(user => user.Address, addressBuilder =>
            {
                addressBuilder.Property(address => address.Country)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.State)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.City)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.Street)
                    .HasColumnType("varchar(25)");
            });

            userBuilder.Property(user => user.Tags)
                .HasColumnType("jsonb")
                .IsRequired();

            userBuilder.OwnsMany(user => user.Cards, cardBuilder =>
            {
                cardBuilder.HasKey(card => card.Id);
                cardBuilder.Property(card => card.Name)
                    .HasColumnType("varchar(25)");
            });

            return modelBuilder;
        }
    }
}