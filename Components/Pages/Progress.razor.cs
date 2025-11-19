using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class ProgressBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;

    protected List<StepProgress>? truckingSteps;
    protected int truckingCompletedCount;

    protected List<StepProgress>? pokemonSteps;
    protected int pokemonCompletedCount;

    protected List<StepProgress>? adminSteps;
    protected int adminCompletedCount;

    protected override async Task OnInitializedAsync()
    {
        await LoadProgressAsync();
    }

    protected async Task LoadProgressAsync()
    {
        truckingSteps = await ProgressService.GetAllStepsAsync("trucking");
        truckingCompletedCount = await ProgressService.GetCompletedCountAsync("trucking");

        pokemonSteps = await ProgressService.GetAllStepsAsync("pokemon");
        pokemonCompletedCount = await ProgressService.GetCompletedCountAsync("pokemon");

        adminSteps = await ProgressService.GetAllStepsAsync("admin");
        adminCompletedCount = await ProgressService.GetCompletedCountAsync("admin");
    }

    protected async Task ToggleStep(string projectKey, int stepNumber)
    {
        var step = projectKey switch
        {
            "trucking" => truckingSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
            "pokemon" => pokemonSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
            "admin" => adminSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
            _ => null
        };

        if (step != null)
        {
            if (step.IsComplete)
                await ProgressService.MarkStepIncompleteAsync(projectKey, stepNumber);
            else
                await ProgressService.MarkStepCompleteAsync(projectKey, stepNumber);

            await LoadProgressAsync();
        }
    }
}
