# Step 13: Dashboard & Reports

## Overview

This step teaches students how to build a dashboard to visualize business data using LINQ aggregation and conditional styling. Students learn to display KPIs, filter data, and create interactive reports.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step13`)
- `Example.razor.cs` - Code-behind with dashboard logic and aggregations
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step13`

## What Students Learn

1. Displaying summary statistics and KPIs (Key Performance Indicators)
2. Filtering and sorting data dynamically with LINQ
3. Using conditional CSS classes for visual status indicators
4. Aggregating data with Sum(), Count(), Average(), GroupBy()
5. Loading related data efficiently with Include()

## Key Concepts

- **LINQ Aggregation** - Sum, Count, Average, GroupBy for data summaries
- **Conditional Styling** - Dynamic CSS classes based on data values
- **KPI Design** - Displaying key metrics prominently
- **Include()** - Eagerly loading related entities to avoid N+1 queries

## Architecture

- **Dashboard Component**: Displays KPI cards and data tables
- **LINQ Queries**: Aggregate data from DbContext
- **Helper Methods**: Calculate CSS classes based on status
- **DbContext Factory**: Create context instances for queries

## Prerequisites

- Step 12: Pay & Expense Calculation (understanding of business calculations)

## Next Steps

- Congratulations! You've completed the Trucking tutorial series! 🎉

## Code Structure

### Loading Dashboard Data

```csharp
protected override async Task OnInitializedAsync()
{
    await LoadData();
}

private async Task LoadData()
{
    // Load routes with related data
    routes = await DbContext.Routes
        .Include(r => r.Driver)
        .Include(r => r.Truck)
        .Where(r => r.Status == RouteStatus.InProgress)
        .ToListAsync();

    // Calculate statistics
    var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    var monthlyRoutes = await DbContext.Routes
        .Where(r => r.ScheduledStartDate >= startOfMonth)
        .ToListAsync();

    stats = new DashboardStats
    {
        ActiveRoutes = routes.Count,
        AvailableDrivers = await DbContext.Drivers.CountAsync(d => d.IsAvailable),
        MonthlyRevenue = monthlyRoutes.Sum(r => r.Revenue),
        TotalDriverPay = monthlyRoutes.Sum(r => r.DriverPay),
        NetProfit = monthlyRoutes.Sum(r => r.Revenue - r.DriverPay - r.EstimatedFuelCost)
    };
}
```

### Conditional Styling

```csharp
private string GetStatusClass(RouteStatus status)
{
    return status switch
    {
        RouteStatus.Scheduled => "status-scheduled",    // Blue
        RouteStatus.InProgress => "status-active",      // Green
        RouteStatus.Delayed => "status-delayed",        // Red
        RouteStatus.Completed => "status-completed",    // Gray
        _ => ""
    };
}

private string GetProfitClass(decimal margin)
{
    return margin switch
    {
        >= 20 => "profit-excellent",  // Green
        >= 10 => "profit-good",       // Yellow
        _ => "profit-poor"             // Red
    };
}
```

### LINQ Aggregation Examples

```csharp
// Sum
decimal totalRevenue = routes.Sum(r => r.Revenue);

// Count
int activeCount = routes.Count(r => r.Status == RouteStatus.InProgress);

// Average
decimal avgPay = routes.Average(r => r.DriverPay);

// Group By
var byDriver = routes
    .GroupBy(r => r.DriverId)
    .Select(g => new {
        DriverId = g.Key,
        TotalRevenue = g.Sum(r => r.Revenue),
        RouteCount = g.Count()
    });
```

## Dashboard Sections

### KPI Cards

- Active Routes
- Available Drivers
- Available Trucks (with maintenance count)
- Monthly Revenue (with profit margin)

### Data Tables

- Active routes with driver, origin→destination, distance, status, revenue
- Filter dropdown for route status
- Sortable columns

### Financial Summary

- Total Revenue (current month)
- Total Driver Pay
- Total Fuel Costs
- Other Expenses
- Net Profit (with color indicator)

## Styling Patterns

- Grid layout for KPI cards (responsive to screen size)
- Status badges with color coding (blue=scheduled, green=active, red=delayed)
- Table styling with borders and hover effects
- Conditional text colors based on profit/loss
- Empty state messaging when no data

## Common Issues & Solutions

### Slow Dashboard Loading

**Issue**: Dashboard takes a long time to load  
**Solution**: Use `Include()` to eagerly load related data and avoid N+1 queries. Consider caching statistics that don't change frequently.

### Incorrect Totals

**Issue**: Sum or Count doesn't match expected values  
**Solution**: Verify your Where() filter is correct. Check for null values that might be excluded. Use `?? 0` for nullable decimals.

### "Object reference not set" Errors

**Issue**: Null reference when accessing navigation properties  
**Solution**: Use Include() to load related entities, or add null checks: `route.Driver?.Name ?? "Unknown"`.

### Performance Issues with Large Datasets

**Issue**: Dashboard slow with thousands of records  
**Solution**: Add pagination, limit queries with Take(), or pre-calculate statistics in a background job.

## LINQ Query Performance Tips

1. **Use Include()** - Load related entities in one query
2. **Filter Early** - Apply Where() before ToListAsync()
3. **Select Only What You Need** - Don't load entire entities if you only need a few properties
4. **Avoid N+1 Queries** - One query with Include() is better than many small queries
5. **Consider Pagination** - Use Skip() and Take() for large result sets

## Related Resources

- [LINQ Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
- [EF Core Performance](https://learn.microsoft.com/en-us/ef/core/performance/)
- [Include() for Related Data](https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager)
- [Aggregate Functions](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable)
