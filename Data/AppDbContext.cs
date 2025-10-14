using BlazorMock.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorMock.Data;

using RouteModel = BlazorMock.Models.Route;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Driver> Drivers { get; set; } = null!;
    public DbSet<Truck> Trucks { get; set; } = null!;
    public DbSet<RouteModel> Routes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Driver>().HasIndex(d => d.LicenseNumber).IsUnique();
        modelBuilder.Entity<Truck>().HasIndex(t => t.TruckNumber).IsUnique();
        modelBuilder.Entity<RouteModel>().HasIndex(r => r.RouteNumber).IsUnique();
    }
}