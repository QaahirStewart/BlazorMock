# Step 12: Pay & Expense Calculation

## Overview

This step teaches students how to create a service for business calculations in a Blazor application. Students learn to calculate driver pay, fuel costs, and profit margins using a dedicated service.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step12`)
- `Example.razor.cs` - Code-behind with progress tracking
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step12`

## What Students Learn

1. Creating a service for complex business calculations
2. Calculating driver pay with experience bonuses and route type premiums
3. Estimating fuel costs based on distance and MPG
4. Computing total route costs and profit margins
5. Using constants for configurable values (fuel price, tax rates, etc.)

## Key Concepts

- **Service Pattern** - Encapsulating business logic in reusable services
- **Business Logic** - Separating calculations from UI components
- **Calculation Methods** - Pure functions for testable math operations
- **Constants** - Centralized configuration values

## Architecture

- **ScheduleService**: Contains all pay and expense calculation methods
- **Service Registration**: Scoped lifetime in Program.cs
- **Pure Functions**: Methods that don't modify state, just calculate and return values
- **Dependency Injection**: Inject service into components that need calculations

## Prerequisites

- Step 11: Assignment Logic & Business Rules (business rule concepts)

## Next Steps

- Step 13: Dashboard & Reports (aggregating and displaying business metrics)

## Code Structure

### ScheduleService

```csharp
public class ScheduleService
{
    private const decimal FuelPricePerGallon = 3.85m;
    private const double AverageMpg = 6.5;
    private const decimal HazmatBonus = 250m;
    private const decimal OversizedBonus = 300m;

    public decimal CalculateDriverPay(Route route, Driver driver)
    {
        // Base pay = hours * hourly rate
        double driveTime = route.GetEstimatedDriveTime();
        decimal basePay = driver.HourlyRate * (decimal)driveTime;

        // Experience bonus (1% per year, max 25%)
        decimal experienceMultiplier = 1.0m + Math.Min(driver.YearsOfExperience * 0.01m, 0.25m);
        decimal payWithExperience = basePay * experienceMultiplier;

        // Route type bonuses
        decimal routeBonus = route.Type switch
        {
            RouteType.Hazmat => HazmatBonus,
            RouteType.Oversized => OversizedBonus,
            RouteType.LongHaul => route.DistanceMiles * 0.15m,
            _ => 0m
        };

        return payWithExperience + routeBonus;
    }

    public decimal EstimateFuelCost(int distanceMiles)
    {
        double gallonsNeeded = distanceMiles / AverageMpg;
        return (decimal)gallonsNeeded * FuelPricePerGallon;
    }

    public decimal CalculateTotalRouteCost(Route route, Driver driver)
    {
        decimal driverPay = CalculateDriverPay(route, driver);
        decimal fuelCost = EstimateFuelCost(route.DistanceMiles);
        decimal otherCosts = route.DistanceMiles * 0.10m; // $0.10/mile

        return driverPay + fuelCost + otherCosts;
    }

    public decimal SuggestMinimumRevenue(Route route, Driver driver)
    {
        decimal totalCost = CalculateTotalRouteCost(route, driver);
        return totalCost * 1.20m; // 20% profit margin
    }
}
```

### Service Registration

```csharp
// Program.cs
builder.Services.AddScoped<ScheduleService>();
```

### Usage in Component

```csharp
@inject ScheduleService ScheduleService

protected async Task CalculateCosts()
{
    estimatedDriverPay = ScheduleService.CalculateDriverPay(route, driver);
    estimatedFuelCost = ScheduleService.EstimateFuelCost(route.DistanceMiles);
    totalCost = ScheduleService.CalculateTotalRouteCost(route, driver);
    suggestedRevenue = ScheduleService.SuggestMinimumRevenue(route, driver);
}
```

## Calculation Formulas

### Driver Pay

```
Base Pay = Hourly Rate × Estimated Drive Time
Experience Bonus = Base Pay × (Years of Experience × 1%, max 25%)
Route Bonus = Hazmat $250 | Oversized $300 | Long Haul $0.15/mile
Total Pay = Base Pay + Experience Bonus + Route Bonus
```

### Fuel Cost

```
Gallons Needed = Distance Miles ÷ Average MPG
Fuel Cost = Gallons Needed × Fuel Price Per Gallon
```

### Total Route Cost

```
Total Cost = Driver Pay + Fuel Cost + Other Costs ($0.10/mile)
```

### Minimum Revenue

```
Minimum Revenue = Total Cost × 1.20 (20% profit margin)
```

## Styling Patterns

- Formula breakdowns in visual cards
- Color-coded sections (pay in green, expenses in amber)
- Real-time calculation display
- Clear labels and formatted currency

## Common Issues & Solutions

### Service Not Found

**Issue**: Dependency injection error when injecting ScheduleService  
**Solution**: Verify `builder.Services.AddScoped<ScheduleService>();` is in Program.cs before `builder.Build()`.

### Incorrect Calculations

**Issue**: Results don't match expected values  
**Solution**: Use `decimal` for money (not `double` or `float`). Check your constants are correct (fuel price, MPG, etc.).

### Rounding Errors

**Issue**: Currency values have too many decimal places  
**Solution**: Use `.ToString("C")` or `.ToString("N2")` for currency formatting. Consider using `Math.Round(value, 2)`.

### Constants Not Updating

**Issue**: Changing constants doesn't affect calculations  
**Solution**: The service is cached. Restart the app or make constants configurable via appsettings.json.

## Related Resources

- [Dependency Injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Decimal vs Double for Money](https://learn.microsoft.com/en-us/dotnet/api/system.decimal)
- [Math Class](https://learn.microsoft.com/en-us/dotnet/api/system.math)
- [Service Lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes)
