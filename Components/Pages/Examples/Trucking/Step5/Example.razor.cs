using System.ComponentModel.DataAnnotations;
using BlazorMock.Models;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step5;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected Driver driver = new();
    protected bool submitted;
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 5);
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
        await ProgressService.MarkStepCompleteAsync("trucking", 5);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 5);
        isComplete = false;
    }

    protected void HandleValidSubmit()
    {
        submitted = true;
    }

    protected void ResetForm()
    {
        driver = new();
        submitted = false;
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

    public class Driver
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be less than 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "CDL number is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "CDL number must be between 4 and 20 characters")]
        public string CdlNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60")]
        public int? YearsExperience { get; set; }

        [Required(ErrorMessage = "License level is required")]
        public LicenseLevel? LicenseLevel { get; set; }

        public bool HasHazmatEndorsement { get; set; }
    }
}
