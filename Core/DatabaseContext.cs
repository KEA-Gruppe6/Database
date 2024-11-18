using Database_project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Departure> Departures { get; set; }
    public DbSet<Luggage> Luggage { get; set; }
    public DbSet<Maintenance> Maintenances { get; set; }
    public DbSet<Plane> Planes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=host.docker.internal;Database=AirportDB;User ID=sa;Password=Password123;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True;",
                options => options.MigrationsAssembly("AirTravel"))
                .UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departure>(entity =>
        {
            entity.HasKey(e => e.DepartureId);
            entity.HasOne(d => d.DepartureAirport)
                .WithMany()
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(d => d.ArrivalAirport)
                .WithMany()
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<Plane>()
            .HasOne(p => p.Airline)
            .WithMany(a => a.Planes)
            .HasForeignKey(p => p.AirlineId);

        base.OnModelCreating(modelBuilder);
    }
}