using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMock.Models;

/// <summary>
/// Represents a delivery route assignment.
/// Links a driver and truck together for a specific delivery job.
/// </summary>
public class Route
{
    /// <summary>
    /// Unique identifier for the route (Primary Key).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Route number for tracking and reference.
    /// Must be unique and is required.
    /// </summary>
    [Required(ErrorMessage = "Route number is required")]
    [StringLength(50)]
    public string RouteNumber { get; set; } = string.Empty;

    /// <summary>
    /// Starting location/city for the route.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Destination location/city for the route.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Total distance of the route in miles.
    /// Used for calculating costs and drive time.
    /// </summary>
    [Range(0, 5000, ErrorMessage = "Distance must be between 0 and 5,000 miles")]
    public int DistanceMiles { get; set; }

    /// <summary>
    /// Type/category of route based on cargo and requirements.
    /// Determines pay rates and special requirements.
    /// </summary>
    [Required]
    public RouteType Type { get; set; }

    /// <summary>
    /// Current status of the route in its lifecycle.
    /// </summary>
    [Required]
    public RouteStatus Status { get; set; } = RouteStatus.Scheduled;

    /// <summary>
    /// Scheduled start date and time for the route.
    /// </summary>
    [Required]
    public DateTime ScheduledStartDate { get; set; }

    /// <summary>
    /// Estimated completion date and time for the route.
    /// Calculated based on distance and average speed.
    /// </summary>
    public DateTime EstimatedEndDate { get; set; }

    /// <summary>
    /// Actual start date and time (set when route begins).
    /// Null until the route actually starts.
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// Actual completion date and time (set when route ends).
    /// Null until the route is completed.
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Foreign key: ID of the assigned driver.
    /// </summary>
    [Required]
    public int DriverId { get; set; }

    /// <summary>
    /// Navigation property: The driver assigned to this route.
    /// </summary>
    [ForeignKey(nameof(DriverId))]
    public Driver Driver { get; set; } = null!;

    /// <summary>
    /// Foreign key: ID of the assigned truck.
    /// </summary>
    [Required]
    public int TruckId { get; set; }

    /// <summary>
    /// Navigation property: The truck assigned to this route.
    /// </summary>
    [ForeignKey(nameof(TruckId))]
    public Truck Truck { get; set; } = null!;

    /// <summary>
    /// Estimated cost of fuel for the route in USD.
    /// Calculated based on distance and fuel efficiency.
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal EstimatedFuelCost { get; set; }

    /// <summary>
    /// Driver's pay for completing this route in USD.
    /// Calculated based on distance, time, and type.
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal DriverPay { get; set; }

    /// <summary>
    /// Revenue generated from this route in USD.
    /// What the customer pays for the delivery.
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Revenue { get; set; }

    /// <summary>
    /// Additional notes or special instructions for the route.
    /// Optional field for important delivery details.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Calculates the estimated drive time in hours.
    /// Assumes average speed of 55 mph.
    /// </summary>
    public double GetEstimatedDriveTime()
    {
        const double averageSpeedMph = 55.0;
        return DistanceMiles / averageSpeedMph;
    }

    /// <summary>
    /// Calculates the profit margin for this route.
    /// Profit = Revenue - (Driver Pay + Estimated Fuel Cost)
    /// </summary>
    public decimal GetProfitMargin()
    {
        return Revenue - (DriverPay + EstimatedFuelCost);
    }

    /// <summary>
    /// Calculates the profit percentage.
    /// Returns 0 if revenue is 0 to avoid division by zero.
    /// </summary>
    public decimal GetProfitPercentage()
    {
        if (Revenue == 0) return 0;
        return (GetProfitMargin() / Revenue) * 100;
    }

    /// <summary>
    /// Checks if the route is currently in progress.
    /// </summary>
    public bool IsInProgress()
    {
        return Status == RouteStatus.InProgress;
    }

    /// <summary>
    /// Checks if the route is completed.
    /// </summary>
    public bool IsCompleted()
    {
        return Status == RouteStatus.Completed;
    }

    /// <summary>
    /// Validates if the assigned driver can handle this route type.
    /// Some route types require minimum experience levels.
    /// </summary>
    public bool ValidateDriverExperience()
    {
        return Type switch
        {
            RouteType.Standard => true, // Anyone can do standard routes
            RouteType.Hazmat => Driver.YearsOfExperience >= 2, // Hazmat needs 2+ years
            RouteType.Oversized => Driver.YearsOfExperience >= 3, // Oversized needs 3+ years
            RouteType.LongHaul => Driver.YearsOfExperience >= 1, // Long haul needs 1+ year
            _ => true
        };
    }

    /// <summary>
    /// Gets a display-friendly summary of the route.
    /// </summary>
    public string GetRouteSummary()
    {
        return $"{RouteNumber}: {Origin} → {Destination} ({DistanceMiles} mi)";
    }
}

/// <summary>
/// Represents the type/category of delivery route.
/// Determines special requirements and pay rates.
/// </summary>
public enum RouteType
{
    /// <summary>
    /// Standard freight - Normal cargo, no special requirements.
    /// Base pay rate.
    /// </summary>
    [Display(Name = "Standard Freight")]
    Standard = 1,

    /// <summary>
    /// Hazardous materials - Requires special endorsement and training.
    /// Higher pay rate due to risk and regulations.
    /// </summary>
    [Display(Name = "Hazmat (Hazardous Materials)")]
    Hazmat = 2,

    /// <summary>
    /// Oversized loads - Wide/tall/long cargo requiring special permits.
    /// Higher pay rate due to complexity.
    /// </summary>
    [Display(Name = "Oversized Load")]
    Oversized = 3,

    /// <summary>
    /// Long haul routes - Cross-country or multi-day trips.
    /// Higher pay due to time away from home.
    /// </summary>
    [Display(Name = "Long Haul")]
    LongHaul = 4
}

/// <summary>
/// Represents the current status of a route in its lifecycle.
/// Tracks the route from scheduling through completion.
/// </summary>
public enum RouteStatus
{
    /// <summary>
    /// Route is scheduled but hasn't started yet.
    /// Driver and truck are assigned.
    /// </summary>
    [Display(Name = "Scheduled")]
    Scheduled = 1,

    /// <summary>
    /// Route is currently in progress.
    /// Driver is on the road.
    /// </summary>
    [Display(Name = "In Progress")]
    InProgress = 2,

    /// <summary>
    /// Route has been successfully completed.
    /// Delivery made, driver returned.
    /// </summary>
    [Display(Name = "Completed")]
    Completed = 3,

    /// <summary>
    /// Route was cancelled before completion.
    /// Customer cancelled or other issues.
    /// </summary>
    [Display(Name = "Cancelled")]
    Cancelled = 4,

    /// <summary>
    /// Route has encountered a problem.
    /// May include delays, breakdowns, or other issues.
    /// </summary>
    [Display(Name = "Delayed")]
    Delayed = 5
}
