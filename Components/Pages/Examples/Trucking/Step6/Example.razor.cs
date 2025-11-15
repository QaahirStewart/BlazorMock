using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step6;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected string currentUri = string.Empty;
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 6);
        isComplete = step?.IsComplete ?? false;
        currentUri = Navigation.Uri;

        // Subscribe to navigation changes
        Navigation.LocationChanged += OnLocationChanged;
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

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUri = e.Location;
        StateHasChanged();
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("trucking", 6);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 6);
        isComplete = false;
    }

    protected void NavigateToPage(string url)
    {
        Navigation.NavigateTo(url);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        _copyModule?.DisposeAsync();
    }
}
