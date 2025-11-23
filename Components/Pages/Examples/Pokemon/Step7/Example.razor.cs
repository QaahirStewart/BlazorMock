using BlazorMock.Components.Pages.Examples;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step7;

public class ExampleBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;

    protected bool isComplete;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 7);
        isComplete = step?.IsComplete ?? false;
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("pokemon", 7);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 7);
        isComplete = false;
    }
}
