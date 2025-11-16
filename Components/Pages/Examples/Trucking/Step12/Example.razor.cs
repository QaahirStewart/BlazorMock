using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step12;

public partial class Example
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;

    private bool isComplete = false;

    // Interactive demo state (nullable to handle empty inputs)
    private int? demoDistance = 500;
    private decimal? demoHourlyRate = 28m;
    private int? demoExperience = 5;
    private string demoRouteType = "Standard";

    // Calculation constants
    private const decimal FuelPricePerGallon = 3.85m;
    private const double AverageMpg = 6.5;
    private const decimal OtherCostPerMile = 0.10m;
    private const decimal ProfitMargin = 1.20m;

    // Calculated values (with validation to prevent errors)
    private double DriveTime => (demoDistance ?? 0) / 60.0; // Assuming 60 mph average
    private decimal BasePay => (demoHourlyRate ?? 0) * (decimal)DriveTime;
    private decimal ExperienceMultiplier => 1.0m + Math.Min((demoExperience ?? 0) * 0.01m, 0.25m);
    private decimal ExperienceBonus => (BasePay * ExperienceMultiplier) - BasePay;
    private decimal RouteBonus => demoRouteType switch
    {
        "Hazmat" => 250m,
        "Oversized" => 300m,
        "LongHaul" => (demoDistance ?? 0) * 0.15m,
        _ => 0m
    };
    private decimal TotalDriverPay => BasePay + ExperienceBonus + RouteBonus;
    private decimal FuelCost => (demoDistance ?? 0) > 0 ? (decimal)((demoDistance ?? 0) / AverageMpg) * FuelPricePerGallon : 0;
    private decimal OtherCosts => (demoDistance ?? 0) * OtherCostPerMile;
    private decimal TotalCost => TotalDriverPay + FuelCost + OtherCosts;
    private decimal MinimumRevenue => TotalCost * ProfitMargin;
    private decimal Profit => MinimumRevenue - TotalCost;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync(12);
        isComplete = step?.IsComplete ?? false;
    }

    private async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync(12);
        isComplete = true;
    }
}
