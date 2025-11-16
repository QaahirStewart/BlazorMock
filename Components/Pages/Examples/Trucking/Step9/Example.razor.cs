using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step9;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    private IJSObjectReference? _copyModule;

    // Live demo state
    protected List<DemoDriver>? demoDrivers;
    protected string newDriverName = string.Empty;
    protected string newDriverLicense = string.Empty;
    protected decimal newDriverRate = 35.00m;
    private int nextId = 1;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 9);
        isComplete = step?.IsComplete ?? false;

        // Initialize demo data
        demoDrivers = new List<DemoDriver>
        {
            new() { Id = nextId++, Name = "Alice Johnson", LicenseNumber = "CDL-001", HourlyRate = 42.50m },
            new() { Id = nextId++, Name = "Bob Smith", LicenseNumber = "CDL-002", HourlyRate = 38.00m },
            new() { Id = nextId++, Name = "Carol Williams", LicenseNumber = "CDL-003", HourlyRate = 40.00m }
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _copyModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/codeblocks.js");
                await _copyModule.InvokeVoidAsync("enhancePreBlocks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load code block enhancer: {ex.Message}");
            }
        }
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("trucking", 9);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 9);
        isComplete = false;
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }

    protected void AddDemoDriver()
    {
        if (string.IsNullOrWhiteSpace(newDriverName) || string.IsNullOrWhiteSpace(newDriverLicense))
            return;

        demoDrivers?.Add(new DemoDriver
        {
            Id = nextId++,
            Name = newDriverName,
            LicenseNumber = newDriverLicense,
            HourlyRate = newDriverRate
        });

        // Reset form
        newDriverName = string.Empty;
        newDriverLicense = string.Empty;
        newDriverRate = 35.00m;
    }

    protected void DeleteDemoDriver(int id)
    {
        var driver = demoDrivers?.FirstOrDefault(d => d.Id == id);
        if (driver != null)
        {
            demoDrivers?.Remove(driver);
        }
    }

    protected class DemoDriver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
    }
}
