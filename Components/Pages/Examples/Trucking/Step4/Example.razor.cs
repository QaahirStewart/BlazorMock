using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step4;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected int currentCount = 0;
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 4);
        isComplete = step?.IsComplete ?? false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _copyModule = await JS.InvokeAsync<IJSObjectReference>("import", "/js/codeblocks.js");
        }
    }

    protected void Increment()
    {
        currentCount++;
    }

    protected void Decrement()
    {
        currentCount--;
    }

    protected void Reset()
    {
        currentCount = 0;
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("trucking", 4);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 4);
        isComplete = false;
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }
}
