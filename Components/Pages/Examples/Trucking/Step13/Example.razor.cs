using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step13;

public partial class Example
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;

    private bool isComplete = false;
    
    // Demo state
    private string selectedStatus = "All";
    private List<DemoRoute> allRoutes = new();

    // Computed properties using LINQ
    private List<DemoRoute> FilteredRoutes => selectedStatus == "All" 
        ? allRoutes 
        : allRoutes.Where(r => r.Status == selectedStatus).ToList();

    private int TotalDistance => FilteredRoutes.Sum(r => r.Distance);
    private decimal TotalRevenue => FilteredRoutes.Sum(r => r.Revenue);
    private decimal AverageRevenue => FilteredRoutes.Any() ? FilteredRoutes.Average(r => r.Revenue) : 0;
    private int UniqueDrivers => FilteredRoutes.Select(r => r.DriverName).Distinct().Count();

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync(13);
        isComplete = step?.IsComplete ?? false;

        // Initialize demo data
        allRoutes = new List<DemoRoute>
        {
            new DemoRoute { Id = 1001, DriverName = "Sarah Chen", TruckNumber = "TRK-001", Distance = 450, Revenue = 2850m, Status = "Completed" },
            new DemoRoute { Id = 1002, DriverName = "Mike Torres", TruckNumber = "TRK-002", Distance = 680, Revenue = 4250m, Status = "In Progress" },
            new DemoRoute { Id = 1003, DriverName = "Sarah Chen", TruckNumber = "TRK-001", Distance = 320, Revenue = 1920m, Status = "Scheduled" },
            new DemoRoute { Id = 1004, DriverName = "Alex Rivera", TruckNumber = "TRK-003", Distance = 890, Revenue = 5800m, Status = "Delayed" },
            new DemoRoute { Id = 1005, DriverName = "Jamie Lee", TruckNumber = "TRK-004", Distance = 520, Revenue = 3150m, Status = "Completed" },
            new DemoRoute { Id = 1006, DriverName = "Mike Torres", TruckNumber = "TRK-002", Distance = 740, Revenue = 4680m, Status = "Completed" },
            new DemoRoute { Id = 1007, DriverName = "Chris Park", TruckNumber = "TRK-005", Distance = 410, Revenue = 2550m, Status = "In Progress" },
            new DemoRoute { Id = 1008, DriverName = "Alex Rivera", TruckNumber = "TRK-003", Distance = 950, Revenue = 6200m, Status = "Scheduled" },
        };
    }

    private async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync(13);
        isComplete = true;
    }

    private static string GetStatusClass(string status) => status switch
    {
        "Scheduled" => "bg-blue-100 text-blue-700",
        "In Progress" => "bg-yellow-100 text-yellow-700",
        "Completed" => "bg-green-100 text-green-700",
        "Delayed" => "bg-red-100 text-red-700",
        _ => "bg-gray-100 text-gray-700"
    };

    private class DemoRoute
    {
        public int Id { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string TruckNumber { get; set; } = string.Empty;
        public int Distance { get; set; }
        public decimal Revenue { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
