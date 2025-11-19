using System;
using System.Threading.Tasks;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMock.Components.Pages.Examples.AdminDashboard.Step4;

public partial class ExampleBase : ComponentBase, IDisposable
{
    [Inject] public ILearningProgressService LearningProgress { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;

    private IJSObjectReference? _module;
    protected bool isComplete;

    protected override async Task OnInitializedAsync()
    {
        var step = await LearningProgress.GetStepAsync("admin-dashboard", 4);
        isComplete = step?.IsComplete == true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/codeblocks.js");
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("enhancePreBlocks");
            }
        }
    }

    protected async Task MarkComplete()
    {
        await LearningProgress.MarkStepCompleteAsync("admin-dashboard", 4);
        isComplete = true;
        StateHasChanged();
    }

    protected async Task ResetStep()
    {
        await LearningProgress.MarkStepIncompleteAsync("admin-dashboard", 4);
        isComplete = false;
        StateHasChanged();
    }

    public void Dispose()
    {
        _ = _module?.DisposeAsync();
    }
}
