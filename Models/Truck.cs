using System.ComponentModel.DataAnnotations;

namespace BlazorMock.Models;

/// <summary>
/// Represents a commercial truck in the fleet.
/// Contains vehicle details, capacity information, and maintenance status.
/// </summary>
public class Truck
{
    /// <summary>
    /// Unique identifier for the truck (Primary Key).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Truck identification number (like a VIN).
    /// Required and must be unique.
    /// </summary>
    [Required(ErrorMessage = "Truck number is required")]
    [StringLength(50)]
    public string TruckNumber { get; set; } = string.Empty;

    /// <summary>
    /// Make of the truck (e.g., Freightliner, Kenworth, Peterbilt).
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Make { get; set; } = string.Empty;

    /// <summary>
    /// Model of the truck (e.g., Cascadia, T680, 579).
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Year the truck was manufactured.
    /// Must be between 1990 and current year + 1.
    /// </summary>
    [Range(1990, 2030, ErrorMessage = "Year must be between 1990 and 2030")]
    public int Year { get; set; }

    /// <summary>
    /// Class/size category of the truck.
    /// Determines what license level is required to drive it.
    /// </summary>
    [Required]
    public TruckClass Class { get; set; }

    /// <summary>
    /// Maximum weight the truck can carry in pounds.
    /// Includes cargo weight plus truck weight.
    /// </summary>
    [Range(0, 80000, ErrorMessage = "Capacity must be between 0 and 80,000 lbs")]
    public int CapacityLbs { get; set; }

    /// <summary>
    /// Current mileage on the truck's odometer.
    /// Used for scheduling maintenance.
    /// </summary>
    [Range(0, 2000000, ErrorMessage = "Mileage must be between 0 and 2,000,000")]
    public int CurrentMileage { get; set; }

    /// <summary>
    /// Mileage at which the next maintenance is due.
    /// Typically set 10,000-25,000 miles ahead of current mileage.
    /// </summary>
    public int NextMaintenanceMileage { get; set; }

    /// <summary>
    /// Indicates if the truck is currently available for assignments.
    /// False means it's in use, in maintenance, or out of service.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Indicates if the truck is currently undergoing maintenance or repairs.
    /// </summary>
    public bool InMaintenance { get; set; } = false;

    /// <summary>
    /// Date of last maintenance service.
    /// Used for tracking maintenance schedules.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// License plate number of the truck.
    /// Optional field for registration tracking.
    /// </summary>
    [StringLength(20)]
    public string? LicensePlate { get; set; }

    /// <summary>
    /// Current status description or notes about the truck.
    /// E.g., "In shop for brake repair", "Available", "On Route #123"
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property: Collection of routes assigned to this truck.
    /// Represents the one-to-many relationship between Truck and Route.
    /// </summary>
    public ICollection<Route> Routes { get; set; } = new List<Route>();

    /// <summary>
    /// Checks if the truck needs maintenance soon.
    /// Returns true if within 1,000 miles of next maintenance.
    /// </summary>
    public bool NeedsMaintenanceSoon()
    {
        return (NextMaintenanceMileage - CurrentMileage) <= 1000;
    }

    /// <summary>
    /// Checks if the truck is overdue for maintenance.
    /// Returns true if current mileage exceeds next maintenance mileage.
    /// </summary>
    public bool IsMaintenanceOverdue()
    {
        return CurrentMileage >= NextMaintenanceMileage;
    }

    /// <summary>
    /// Gets the minimum license level required to drive this truck.
    /// </summary>
    public LicenseLevel GetRequiredLicenseLevel()
    {
        return Class switch
        {
            TruckClass.Heavy => LicenseLevel.ClassA,
            TruckClass.Medium => LicenseLevel.ClassB,
            TruckClass.Light => LicenseLevel.ClassC,
            _ => LicenseLevel.ClassC
        };
    }

    /// <summary>
    /// Validates if a driver can operate this truck based on license level.
    /// </summary>
    public bool CanBeOperatedBy(Driver driver)
    {
        var requiredLevel = GetRequiredLicenseLevel();
        return driver.LicenseLevel >= requiredLevel;
    }

    /// <summary>
    /// Gets a display-friendly string showing truck details.
    /// </summary>
    public string GetDisplayName()
    {
        return $"{Year} {Make} {Model} (#{TruckNumber})";
    }
}

/// <summary>
/// Represents the size/weight class of commercial trucks.
/// Determines licensing requirements and payload capacity.
/// </summary>
public enum TruckClass
{
    /// <summary>
    /// Light trucks - Under 26,001 lbs GVWR.
    /// Requires Class C license.
    /// Examples: Cargo vans, small box trucks.
    /// </summary>
    [Display(Name = "Light (Under 26,001 lbs)")]
    Light = 1,

    /// <summary>
    /// Medium trucks - 26,001+ lbs GVWR, single unit.
    /// Requires Class B license.
    /// Examples: Straight trucks, large delivery trucks, buses.
    /// </summary>
    [Display(Name = "Medium (26,001+ lbs)")]
    Medium = 2,

    /// <summary>
    /// Heavy trucks - Combination vehicles, tractor-trailers.
    /// Requires Class A license.
    /// Examples: Semi-trucks, tankers, flatbeds with trailers.
    /// </summary>
    [Display(Name = "Heavy (Tractor-Trailers)")]
    Heavy = 3
}
