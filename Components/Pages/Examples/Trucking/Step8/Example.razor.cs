using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step8;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 8);
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
        await ProgressService.MarkStepCompleteAsync("trucking", 8);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 8);
        isComplete = false;
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }
}
