using System.ComponentModel.DataAnnotations;
using BlazorMock.Models;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step7;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected bool showSuccess;
    protected SampleDriver sampleDriver = new();
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 7);
        isComplete = step?.IsComplete ?? false;
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
        await ProgressService.MarkStepCompleteAsync("trucking", 7);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 7);
        isComplete = false;
    }

    protected void HandleSubmit()
    {
        showSuccess = true;
    }

    protected static string GetLicenseDisplay(LicenseLevel level)
    {
        var member = typeof(LicenseLevel).GetMember(level.ToString()).FirstOrDefault();
        if (member is not null)
        {
            var displayAttr = (DisplayAttribute?)Attribute.GetCustomAttribute(member, typeof(DisplayAttribute));
            if (displayAttr?.Name is string name && !string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
        }
        return level.ToString();
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }

    protected class SampleDriver
    {
        [Required(ErrorMessage = "Driver name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "License number is required")]
        [StringLength(20, ErrorMessage = "License number cannot exceed 20 characters")]
        public string LicenseNumber { get; set; } = string.Empty;

        public LicenseLevel LicenseLevel { get; set; } = LicenseLevel.ClassA;

        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int YearsOfExperience { get; set; } = 5;

        [Range(0, 200, ErrorMessage = "Hourly rate must be between $0 and $200")]
        public decimal HourlyRate { get; set; } = 35.50m;

        public bool IsAvailable { get; set; } = true;
    }
}
