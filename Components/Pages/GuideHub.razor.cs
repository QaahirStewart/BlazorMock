using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class GuideHubBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected ILearningGuideService GuideService { get; set; } = default!;

    protected List<StepProgress> steps = new();
    protected int completedCount;
    protected int pokemonCompletedCount;
    protected int adminCompletedCount;
    protected int pokemonTotalCount;
    protected int adminTotalCount;

    protected override async Task OnInitializedAsync()
    {
        steps = await ProgressService.GetAllStepsAsync();
        completedCount = await ProgressService.GetCompletedCountAsync();
        pokemonCompletedCount = await ProgressService.GetCompletedCountAsync("pokemon");
        adminCompletedCount = await ProgressService.GetCompletedCountAsync("admin");

        var pokemonSteps = await ProgressService.GetAllStepsAsync("pokemon");
        pokemonTotalCount = pokemonSteps.Count;
        var adminSteps = await ProgressService.GetAllStepsAsync("admin");
        adminTotalCount = adminSteps.Count;
    }
}
