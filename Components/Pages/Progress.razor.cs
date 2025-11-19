using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class ProgressBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;

    protected const string TruckingProjectKey = "trucking";
    protected const string PokemonProjectKey = "pokemon";
    protected const string AdminProjectKey = "admin-dashboard";

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
        truckingSteps = await ProgressService.GetAllStepsAsync(TruckingProjectKey);
        truckingCompletedCount = await ProgressService.GetCompletedCountAsync(TruckingProjectKey);

        pokemonSteps = await ProgressService.GetAllStepsAsync(PokemonProjectKey);
        pokemonCompletedCount = await ProgressService.GetCompletedCountAsync(PokemonProjectKey);

        adminSteps = await ProgressService.GetAllStepsAsync(AdminProjectKey);
        adminCompletedCount = await ProgressService.GetCompletedCountAsync(AdminProjectKey);
    }

    protected async Task ToggleStep(string projectKey, int stepNumber)
    {
        var step = projectKey switch
        {
            TruckingProjectKey => truckingSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
            PokemonProjectKey => pokemonSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
            AdminProjectKey => adminSteps?.FirstOrDefault(s => s.StepNumber == stepNumber),
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
