using BlazorMock.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorMock.Data;

using RouteModel = BlazorMock.Models.Route;

public static class DevDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // If there is already data, skip seeding to avoid duplicates
        var hasAny = await db.Drivers.AnyAsync() || await db.Trucks.AnyAsync() || await db.Routes.AnyAsync();
        if (hasAny)
            return;

        // Seed Drivers
        var drivers = new List<Driver>
        {
            new Driver { Name = "Ava Johnson", LicenseNumber = "DL-A001", LicenseLevel = LicenseLevel.ClassA, YearsOfExperience = 6, HourlyRate = 42.50m, Email = "ava@example.com", PhoneNumber = "555-1010", HireDate = DateTime.Today.AddYears(-4), IsAvailable = true },
            new Driver { Name = "Ben Carter", LicenseNumber = "DL-B002", LicenseLevel = LicenseLevel.ClassB, YearsOfExperience = 3, HourlyRate = 34.00m, Email = "ben@example.com", PhoneNumber = "555-2020", HireDate = DateTime.Today.AddYears(-2), IsAvailable = true },
            new Driver { Name = "Chloe Smith", LicenseNumber = "DL-C003", LicenseLevel = LicenseLevel.ClassC, YearsOfExperience = 1, HourlyRate = 26.75m, Email = "chloe@example.com", PhoneNumber = "555-3030", HireDate = DateTime.Today.AddYears(-1), IsAvailable = true },
        };

        db.Drivers.AddRange(drivers);
        await db.SaveChangesAsync();

        // Seed Trucks
        var trucks = new List<Truck>
        {
            new Truck { TruckNumber = "TRK-100", Make = "Freightliner", Model = "Cascadia", Year = DateTime.Now.Year - 2, Class = TruckClass.Heavy, CapacityLbs = 80000, CurrentMileage = 210000, NextMaintenanceMileage = 220000, IsAvailable = true, LicensePlate = "ABC-100" },
            new Truck { TruckNumber = "TRK-200", Make = "Kenworth", Model = "T680", Year = DateTime.Now.Year - 1, Class = TruckClass.Medium, CapacityLbs = 54000, CurrentMileage = 98000, NextMaintenanceMileage = 110000, IsAvailable = true, LicensePlate = "DEF-200" },
            new Truck { TruckNumber = "TRK-300", Make = "Ford", Model = "E-Transit", Year = DateTime.Now.Year, Class = TruckClass.Light, CapacityLbs = 9000, CurrentMileage = 12000, NextMaintenanceMileage = 25000, IsAvailable = true, LicensePlate = "GHI-300" },
        };

        db.Trucks.AddRange(trucks);
        await db.SaveChangesAsync();

        // Map drivers to trucks for valid assignments
        var ava = drivers[0]; // Class A
        var ben = drivers[1]; // Class B
        var chloe = drivers[2]; // Class C

        var heavy = trucks[0];
        var medium = trucks[1];
        var light = trucks[2];

        // Seed Routes (ensure valid license/truck combos)
        var today = DateTime.Today;
        var routes = new List<RouteModel>
        {
            new RouteModel {
                RouteNumber = "R-1001", Origin = "Los Angeles", Destination = "Phoenix", DistanceMiles = 372,
                Type = RouteType.LongHaul, Status = RouteStatus.Scheduled,
                ScheduledStartDate = today.AddDays(1).AddHours(8), EstimatedEndDate = today.AddDays(1).AddHours(16),
                DriverId = ava.Id, TruckId = heavy.Id,
                EstimatedFuelCost = 450.00m, DriverPay = 380.00m, Revenue = 1500.00m
            },
            new RouteModel {
                RouteNumber = "R-2002", Origin = "Dallas", Destination = "Austin", DistanceMiles = 195,
                Type = RouteType.Standard, Status = RouteStatus.Scheduled,
                ScheduledStartDate = today.AddDays(2).AddHours(7), EstimatedEndDate = today.AddDays(2).AddHours(13),
                DriverId = ben.Id, TruckId = medium.Id,
                EstimatedFuelCost = 180.00m, DriverPay = 220.00m, Revenue = 700.00m
            },
            new RouteModel {
                RouteNumber = "R-3003", Origin = "Seattle", Destination = "Tacoma", DistanceMiles = 35,
                Type = RouteType.Standard, Status = RouteStatus.Scheduled,
                ScheduledStartDate = today.AddDays(1).AddHours(9), EstimatedEndDate = today.AddDays(1).AddHours(11),
                DriverId = chloe.Id, TruckId = light.Id,
                EstimatedFuelCost = 25.00m, DriverPay = 60.00m, Revenue = 180.00m
            }
        };

        db.Routes.AddRange(routes);
        await db.SaveChangesAsync();
    }

    public static async Task ResetAsync(AppDbContext db)
    {
        // Dev-only reset: drop and recreate schema, then seed fresh data
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
        await SeedAsync(db);
    }
}
