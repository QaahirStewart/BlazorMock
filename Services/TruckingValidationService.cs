using BlazorMock.Models;
using RouteModel = BlazorMock.Models.Route;

namespace BlazorMock.Services;

/// <summary>
/// Business validation rules for trucking operations (Step 11)
/// </summary>
public class TruckingValidationService
{
    public List<string> ValidateAssignment(Driver? driver, Truck? truck, RouteModel? route)
    {
        var errors = new List<string>();

        if (driver == null)
        {
            errors.Add("Driver is required");
            return errors;
        }

        if (truck == null)
        {
            errors.Add("Truck is required");
            return errors;
        }

        if (route == null)
        {
            errors.Add("Route is required");
            return errors;
        }

        // License validation
        if (!truck.CanBeOperatedBy(driver))
        {
            errors.Add($"Driver needs {truck.Class} license to operate this truck");
        }

        // Availability validation
        if (!driver.IsAvailable)
        {
            errors.Add($"Driver {driver.Name} is currently unavailable");
        }

        if (!truck.IsAvailable)
        {
            errors.Add($"Truck {truck.TruckNumber} is currently unavailable");
        }

        if (truck.InMaintenance)
        {
            errors.Add($"Truck {truck.TruckNumber} is in maintenance");
        }

        // Route type validation
        if (route.Type == RouteType.Hazmat && driver.LicenseLevel != LicenseLevel.ClassA)
        {
            errors.Add("Hazmat routes require CDL-A license");
        }

        if (route.Type == RouteType.Oversized && driver.YearsOfExperience < 3)
        {
            errors.Add("Oversized routes require at least 3 years experience");
        }

        return errors;
    }

    public bool CanStartRoute(RouteModel route)
    {
        return route.Status == RouteStatus.Scheduled &&
               route.Driver != null &&
               route.Truck != null &&
               route.Driver.IsAvailable &&
               route.Truck.IsAvailable &&
               !route.Truck.InMaintenance;
    }

    public bool CanCompleteRoute(RouteModel route)
    {
        return route.Status == RouteStatus.InProgress;
    }

    public bool CanCancelRoute(RouteModel route)
    {
        return route.Status == RouteStatus.Scheduled || route.Status == RouteStatus.InProgress;
    }
}

