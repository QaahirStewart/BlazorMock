using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step1;

public partial class ExampleBase : ComponentBase, IDisposable
{
    // Dependency Injection
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    // State
    protected bool isComplete;
    protected IJSObjectReference? _copyModule;

    // Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 1);
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

    // Event Handlers
    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("trucking", 1);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 1);
        isComplete = false;
    }

    // Cleanup
    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }
}
