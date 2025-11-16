using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Admin.Step0;

public class ExampleBase : ComponentBase
{
    [Inject]
    public ILearningProgressService LearningProgressService { get; set; } = default!;

    protected bool isComplete;

    private const string ProjectKey = "admin";
    private const int StepNumber = 0;

    protected override async Task OnInitializedAsync()
    {
        var progress = await LearningProgressService.GetStepAsync(ProjectKey, StepNumber);
        isComplete = progress?.IsComplete ?? false;
    }

    protected async Task MarkComplete()
    {
        await LearningProgressService.MarkStepCompleteAsync(ProjectKey, StepNumber);
        isComplete = true;
        StateHasChanged();
    }

    protected async Task ResetStep()
    {
        await LearningProgressService.MarkStepIncompleteAsync(ProjectKey, StepNumber);
        isComplete = false;
        StateHasChanged();
    }
}
