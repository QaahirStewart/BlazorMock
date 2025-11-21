using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class PokemonGuideBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected ILearningGuideService GuideService { get; set; } = default!;

    protected List<StepProgress>? _steps;
    protected int _completedCount;
    protected IReadOnlyList<Phase>? _phases;

    protected override async Task OnInitializedAsync()
    {
        _steps = await ProgressService.GetAllStepsAsync("pokemon");
        _completedCount = await ProgressService.GetCompletedCountAsync("pokemon");
        _phases = GuideService.GetPhases("pokemon");
    }

    protected static string PhaseHoverBorder(string id) => id switch
    {
        "pokemon-phase-1" => "hover:border-yellow-400",
        "pokemon-phase-2" => "hover:border-blue-400",
        "pokemon-phase-3" => "hover:border-emerald-400",
        _ => "hover:border-gray-300"
    };

    protected static string PhasePillColors(string id) => id switch
    {
        "pokemon-phase-1" => "bg-yellow-50 text-yellow-700 border border-yellow-200",
        "pokemon-phase-2" => "bg-blue-50 text-blue-700 border border-blue-200",
        "pokemon-phase-3" => "bg-emerald-50 text-emerald-700 border border-emerald-200",
        _ => "bg-gray-50 text-gray-700 border border-gray-200"
    };

    public static string PhaseGradient(string id) => id switch
    {
        "pokemon-phase-1" => "bg-gradient-to-r from-yellow-100 to-yellow-300",
        "pokemon-phase-2" => "bg-gradient-to-r from-blue-100 to-blue-300",
        "pokemon-phase-3" => "bg-gradient-to-r from-emerald-100 to-emerald-300",
        _ => "bg-gradient-to-r from-gray-100 to-gray-300"
    };

    protected async Task ToggleStep(int stepNumber)
    {
        if (_steps is null)
        {
            return;
        }

        var step = _steps.FirstOrDefault(s => s.StepNumber == stepNumber);
        if (step is null)
        {
            return;
        }

        if (step.IsComplete)
        {
            await ProgressService.MarkStepIncompleteAsync("pokemon", stepNumber);
        }
        else
        {
            await ProgressService.MarkStepCompleteAsync("pokemon", stepNumber);
        }

        _steps = await ProgressService.GetAllStepsAsync("pokemon");
        _completedCount = await ProgressService.GetCompletedCountAsync("pokemon");
    }
}
