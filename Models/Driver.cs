using System.ComponentModel.DataAnnotations;

namespace BlazorMock.Models;

/// <summary>
/// Represents a truck driver in the system.
/// Contains personal information, license details, and availability status.
/// </summary>
public class Driver
{
    /// <summary>
    /// Unique identifier for the driver (Primary Key).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the driver.
    /// Required field with maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "Driver name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Driver's license number.
    /// Required and must be unique in the system.
    /// </summary>
    [Required(ErrorMessage = "License number is required")]
    [StringLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Type of commercial driver's license held.
    /// Determines what classes of trucks can be driven.
    /// </summary>
    [Required]
    public LicenseLevel LicenseLevel { get; set; }

    /// <summary>
    /// Years of professional driving experience.
    /// Must be between 0 and 50 years.
    /// </summary>
    [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// Hourly pay rate for the driver in USD.
    /// Typically ranges from $15 to $50 per hour.
    /// </summary>
    [Range(0, 200, ErrorMessage = "Pay rate must be between $0 and $200")]
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// Indicates if the driver is currently available for assignments.
    /// False means they're on a route, on leave, or otherwise unavailable.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Email address for contacting the driver.
    /// Optional field with email format validation.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// Phone number for contacting the driver.
    /// Optional field, can include formatting characters.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Date when the driver was hired.
    /// Used for calculating seniority and benefits.
    /// </summary>
    public DateTime HireDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Navigation property: Collection of routes assigned to this driver.
    /// Represents the one-to-many relationship between Driver and Route.
    /// </summary>
    public ICollection<Route> Routes { get; set; } = new List<Route>();

    /// <summary>
    /// Calculates the driver's effective experience level for assignment validation.
    /// Takes into account both years of experience and license level.
    /// </summary>
    public int GetEffectiveExperience()
    {
        // Add bonus experience based on license level
        int bonus = LicenseLevel switch
        {
            LicenseLevel.ClassA => 5,  // Class A drivers get 5 bonus years
            LicenseLevel.ClassB => 3,  // Class B drivers get 3 bonus years
            LicenseLevel.ClassC => 0,  // Class C drivers get no bonus
            _ => 0
        };
        
        return YearsOfExperience + bonus;
    }

    /// <summary>
    /// Determines if the driver can be assigned to a specific truck class.
    /// </summary>
    public bool CanDriveTruckClass(TruckClass truckClass)
    {
        return truckClass switch
        {
            TruckClass.Heavy => LicenseLevel == LicenseLevel.ClassA,
            TruckClass.Medium => LicenseLevel >= LicenseLevel.ClassB,
            TruckClass.Light => true, // Anyone can drive light trucks
            _ => false
        };
    }
}

/// <summary>
/// Represents the different classes of commercial driver's licenses.
/// Ordered from highest (Class A) to lowest (Class C) capability.
/// </summary>
public enum LicenseLevel
{
    /// <summary>
    /// Class C license - Light vehicles only (under 26,001 lbs).
    /// Cannot haul hazardous materials or tow heavy trailers.
    /// </summary>
    [Display(Name = "Class C (Light Vehicles)")]
    ClassC = 1,

    /// <summary>
    /// Class B license - Medium vehicles (26,001+ lbs).
    /// Can drive straight trucks, large buses, and segmented buses.
    /// Cannot tow trailers over 10,000 lbs.
    /// </summary>
    [Display(Name = "Class B (Medium Vehicles)")]
    ClassB = 2,

    /// <summary>
    /// Class A license - Heavy vehicles (all commercial vehicles).
    /// Can drive tractor-trailers, tanker trucks, and flatbeds.
    /// Highest level of commercial license.
    /// </summary>
    [Display(Name = "Class A (Heavy Vehicles)")]
    ClassA = 3
}
