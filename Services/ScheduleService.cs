using BlazorMock.Models;
using RouteModel = BlazorMock.Models.Route;

namespace BlazorMock.Services;

/// <summary>
/// Business logic service for route scheduling calculations (Step 12)
/// </summary>
public class ScheduleService
{
    public const decimal FuelPricePerGallon = 3.85m;
    public const double AverageMpg = 6.5;
    private const decimal OtherCostPerMile = 0.10m;
    private const decimal ProfitMargin = 1.20m;

    /// <summary>
    /// Calculate driver pay for a route (with driver model)
    /// </summary>
    public decimal CalculateDriverPay(Driver driver, RouteModel route)
    {
        // Estimate hours: 50 mph average speed
        var hoursEstimated = (int)Math.Ceiling(route.DistanceMiles / 50.0);
        // Base hourly rate
        var hourlyRate = 25m;
        
        return CalculateDriverPay(hoursEstimated, hourlyRate, driver.YearsOfExperience, route.Type, route.DistanceMiles);
    }

    public decimal CalculateDriverPay(int hoursEstimated, decimal hourlyRate, int yearsExperience, RouteType routeType, int distanceMiles)
    {
        // Base pay
        var basePay = hoursEstimated * hourlyRate;

        // Experience bonus: 1% per year, max 25%
        var experienceMultiplier = 1.0m + Math.Min(yearsExperience * 0.01m, 0.25m);
        var experienceBonus = (basePay * experienceMultiplier) - basePay;

        // Route type bonuses
        var routeBonus = routeType switch
        {
            RouteType.Hazmat => 250m,
            RouteType.Oversized => 300m,
            RouteType.LongHaul => distanceMiles * 0.15m,
            _ => 0m
        };

        return basePay + experienceBonus + routeBonus;
    }

    public decimal EstimateFuelCost(int distanceMiles)
    {
        var gallons = distanceMiles / AverageMpg;
        return (decimal)gallons * FuelPricePerGallon;
    }

    /// <summary>
    /// Calculate total route cost (with driver model)
    /// </summary>
    public decimal CalculateTotalRouteCost(Driver driver, RouteModel route)
    {
        var driverPay = CalculateDriverPay(driver, route);
        return CalculateTotalRouteCost(driverPay, route.DistanceMiles);
    }

    public decimal CalculateTotalRouteCost(decimal driverPay, int distanceMiles)
    {
        var fuelCost = EstimateFuelCost(distanceMiles);
        var otherCosts = distanceMiles * OtherCostPerMile;
        return driverPay + fuelCost + otherCosts;
    }

    /// <summary>
    /// Suggest minimum revenue (with driver model)
    /// </summary>
    public decimal SuggestMinimumRevenue(Driver driver, RouteModel route)
    {
        var totalCost = CalculateTotalRouteCost(driver, route);
        return SuggestMinimumRevenue(totalCost);
    }

    public decimal SuggestMinimumRevenue(decimal totalCost)
    {
        return totalCost * ProfitMargin;
    }
}
