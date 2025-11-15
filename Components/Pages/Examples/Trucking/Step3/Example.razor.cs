using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step3;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    private IJSObjectReference? _copyModule;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("trucking", 3);
        isComplete = step?.IsComplete ?? false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _copyModule = await JS.InvokeAsync<IJSObjectReference>("import", "/js/codeblocks.js");
        }
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("trucking", 3);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("trucking", 3);
        isComplete = false;
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }
}
