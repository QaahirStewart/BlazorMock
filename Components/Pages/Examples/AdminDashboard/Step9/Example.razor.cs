using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.AdminDashboard.Step9;

public partial class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
    private string _demoFrameSrc = BuildDemoFrameSrc();

    protected string DemoFrameSrc => _demoFrameSrc;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("admin-dashboard", 9);
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
        await ProgressService.MarkStepCompleteAsync("admin-dashboard", 9);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("admin-dashboard", 9);
        isComplete = false;
    }

    protected void RefreshDemoFrame()
    {
        _demoFrameSrc = BuildDemoFrameSrc();
    }

    protected Task LaunchAdminDemo() => LaunchDemoAsync("admin");

    protected Task LaunchPaidDemo() => LaunchDemoAsync("paid");

    protected Task LaunchFreeDemo() => LaunchDemoAsync("free");

    private async Task LaunchDemoAsync(string role)
    {
        var safeRole = role.ToLowerInvariant() switch
        {
            "admin" => "admin",
            "paid" => "paid",
            _ => "free"
        };

        var redirect = Uri.EscapeDataString("/admin/dashboard");
        var targetUrl = $"/signin?demo={safeRole}&redirect={redirect}";

        try
        {
            await JS.InvokeVoidAsync("open", targetUrl, "_blank", "noopener");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to launch demo window: {ex.Message}");
        }
    }

    private static string BuildDemoFrameSrc() => $"/admin/dashboard?embed=docs&ts={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }
}
