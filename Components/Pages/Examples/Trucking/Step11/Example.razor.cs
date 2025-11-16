using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step11;

public partial class Example
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;

    private bool isComplete = false;

    // Demo State
    private enum DemoLicenseLevel { ClassC = 1, ClassB = 2, ClassA = 3 }
    private enum DemoTruckClass { Light, Medium, Heavy }
    private enum DemoRouteType { Local, LongHaul, Hazmat, Oversized }

    private class DemoDriver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DemoLicenseLevel LicenseLevel { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    private class DemoTruck
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public DemoTruckClass Class { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    private List<DemoDriver> demoDrivers = new();
    private List<DemoTruck> demoTrucks = new();
    private List<DemoRouteType> demoRouteTypes = Enum.GetValues<DemoRouteType>().ToList();
    private int? demoSelectedDriverId;
    private int? demoSelectedTruckId;
    private DemoRouteType? demoSelectedRouteType;
    private bool demoIsValid;
    private List<string> demoWarnings = new();

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync(11);
        isComplete = step?.IsComplete ?? false;
        
        SeedDemoData();
    }

    private async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync(11);
        isComplete = true;
    }

    private void SeedDemoData()
    {
        demoDrivers = new()
        {
            new DemoDriver { Id = 1, Name = "Alex", LicenseLevel = DemoLicenseLevel.ClassA, YearsOfExperience = 5, IsAvailable = true },
            new DemoDriver { Id = 2, Name = "Sam", LicenseLevel = DemoLicenseLevel.ClassB, YearsOfExperience = 2, IsAvailable = true },
            new DemoDriver { Id = 3, Name = "Riley", LicenseLevel = DemoLicenseLevel.ClassC, YearsOfExperience = 1, IsAvailable = true },
        };

        demoTrucks = new()
        {
            new DemoTruck { Id = 1, DisplayName = "Freightliner FL-01", Class = DemoTruckClass.Heavy, IsAvailable = true },
            new DemoTruck { Id = 2, DisplayName = "Volvo VNL-22", Class = DemoTruckClass.Medium, IsAvailable = true },
            new DemoTruck { Id = 3, DisplayName = "Ford F-150", Class = DemoTruckClass.Light, IsAvailable = true },
        };
    }

    private void OnDemoDriverChanged(ChangeEventArgs e)
    {
        demoSelectedDriverId = int.TryParse(Convert.ToString(e.Value), out var id) ? id : null;
        ValidateDemo();
    }

    private void OnDemoTruckChanged(ChangeEventArgs e)
    {
        demoSelectedTruckId = int.TryParse(Convert.ToString(e.Value), out var id) ? id : null;
        ValidateDemo();
    }

    private void OnDemoRouteTypeChanged(ChangeEventArgs e)
    {
        var val = Convert.ToString(e.Value);
        demoSelectedRouteType = string.IsNullOrWhiteSpace(val) ? null : Enum.Parse<DemoRouteType>(val);
        ValidateDemo();
    }

    private void ResetDemo()
    {
        demoSelectedDriverId = null;
        demoSelectedTruckId = null;
        demoSelectedRouteType = null;
        demoWarnings.Clear();
        demoIsValid = false;
    }

    private void ValidateDemo()
    {
        demoWarnings.Clear();
        demoIsValid = true;

        var driver = demoSelectedDriverId.HasValue ? demoDrivers.FirstOrDefault(d => d.Id == demoSelectedDriverId.Value) : null;
        var truck = demoSelectedTruckId.HasValue ? demoTrucks.FirstOrDefault(t => t.Id == demoSelectedTruckId.Value) : null;

        if (driver is null || truck is null)
        {
            demoIsValid = false;
            return;
        }

        bool licenseOk = truck.Class switch
        {
            DemoTruckClass.Heavy => driver.LicenseLevel == DemoLicenseLevel.ClassA,
            DemoTruckClass.Medium => driver.LicenseLevel >= DemoLicenseLevel.ClassB,
            _ => true
        };
        
        if (!licenseOk)
        {
            demoWarnings.Add($"❌ {driver.Name} has {driver.LicenseLevel}, but {truck.DisplayName} requires higher license");
            demoIsValid = false;
        }

        if (demoSelectedRouteType is null)
        {
            demoWarnings.Add("⚠️ Please select a route type");
            demoIsValid = false;
            return;
        }

        var routeType = demoSelectedRouteType.Value;
        bool hasExperience = routeType switch
        {
            DemoRouteType.Hazmat => driver.YearsOfExperience >= 2,
            DemoRouteType.Oversized => driver.YearsOfExperience >= 3,
            DemoRouteType.LongHaul => driver.YearsOfExperience >= 1,
            _ => true
        };
        
        if (!hasExperience)
        {
            demoWarnings.Add($"⚠️ {driver.Name} has {driver.YearsOfExperience} years; {routeType} routes need more experience");
            demoIsValid = false;
        }

        if (demoIsValid && !demoWarnings.Any())
        {
            demoWarnings.Add("✅ Assignment is valid and ready!");
        }
    }
}
