using Database_project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Flightroute> Flightroutes { get; set; }
    public DbSet<Luggage> Luggage { get; set; }
    public DbSet<Maintenance> Maintenances { get; set; }
    public DbSet<Plane> Planes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flightroute>(entity =>
        {
            entity.HasKey(e => e.FlightrouteId);
            entity.HasOne(d => d.DepartureAirport)
                .WithMany()
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(d => d.ArrivalAirport)
                .WithMany()
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(d => d.Plane)
                .WithMany()
                .HasForeignKey(d => d.PlaneId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Plane>()
            .HasOne(p => p.Airline)
            .WithMany(a => a.Planes)
            .HasForeignKey(p => p.AirlineId);

        modelBuilder.Entity<Airline>()
            .HasMany(a => a.Planes)
            .WithOne(p => p.Airline)
            .HasForeignKey(p => p.AirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }
}