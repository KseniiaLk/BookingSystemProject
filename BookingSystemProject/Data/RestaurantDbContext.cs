using BookingSystemProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemProject.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Table> Tables { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table configuration
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TableNumber).IsRequired();
            entity.Property(e => e.Capacity).IsRequired();
            entity.HasIndex(e => e.TableNumber).IsUnique();
        });

        // Administrator configuration
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        });

        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BookingDate).IsRequired();
            entity.Property(e => e.BookingTime).IsRequired();
            entity.Property(e => e.NumberOfGuests).IsRequired();

            entity.HasOne(e => e.Table)
                .WithMany(t => t.Bookings)
                .HasForeignKey(e => e.TableId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MenuItem configuration
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.BildUrl).HasMaxLength(500);
        });
    }
}
