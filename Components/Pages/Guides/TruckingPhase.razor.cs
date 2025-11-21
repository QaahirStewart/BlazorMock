using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class TruckingPhaseBase : ComponentBase
{
    [Parameter] public string Id { get; set; } = string.Empty;

    [Inject] protected ILearningGuideService GuideService { get; set; } = default!;
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;

    protected Phase? phase;
    protected List<StepProgress> steps = new();
    protected int completedInPhase;
    protected IReadOnlyList<Phase>? phases;

    protected override async Task OnParametersSetAsync()
    {
        phases ??= GuideService.GetPhases("trucking");
        phase = GuideService.GetPhaseById("trucking", Id);
        steps = await ProgressService.GetAllStepsAsync("trucking");
        completedInPhase = phase is null
            ? 0
            : steps.Count(s => s.IsComplete && phase.StepNumbers.Contains(s.StepNumber));
    }

    protected string StepHref(int n) => n == 0 ? "/trucking-examples/step0" : $"/trucking-examples/step{n}";

    protected static string PhasePillColors(string id) => id switch
    {
        "phase-1" => "bg-purple-50 text-purple-700 border border-purple-200",
        "phase-2" => "bg-blue-50 text-blue-700 border border-blue-200",
        "phase-3" => "bg-emerald-50 text-emerald-700 border border-emerald-200",
        _ => "bg-gray-50 text-gray-700 border border-gray-200"
    };

    protected static string PhaseHeaderGradient(string id) => id switch
    {
        "phase-1" => "bg-gradient-to-r from-purple-100 via-indigo-100 to-indigo-200",
        "phase-2" => "bg-gradient-to-r from-blue-100 via-sky-100 to-sky-200",
        "phase-3" => "bg-gradient-to-r from-emerald-100 via-teal-100 to-teal-200",
        _ => "bg-gradient-to-r from-gray-100 to-blue-100"
    };

    protected static string PhaseGradient(string id) => id switch
    {
        "phase-1" => "bg-gradient-to-r from-purple-200 to-purple-400",
        "phase-2" => "bg-gradient-to-r from-blue-200 to-blue-400",
        "phase-3" => "bg-gradient-to-r from-emerald-200 to-emerald-400",
        _ => "bg-gradient-to-r from-gray-200 to-gray-300"
    };

    protected static string PhaseHoverBorder(string id) => id switch
    {
        "phase-1" => "hover:border-purple-400",
        "phase-2" => "hover:border-blue-400",
        "phase-3" => "hover:border-emerald-400",
        _ => "hover:border-gray-300"
    };

        protected static string PhaseRingColor(string id) => id switch
    {
        "phase-1" => "ring-1 ring-yellow-300/70",
        "phase-2" => "ring-1 ring-blue-300/70",
        "phase-3" => "ring-1 ring-emerald-300/70",
        _ => "ring-1 ring-white/70"
    };
}
