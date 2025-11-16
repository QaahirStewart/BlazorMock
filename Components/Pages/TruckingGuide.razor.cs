using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class TruckingGuideBase : ComponentBase
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected ILearningGuideService GuideService { get; set; } = default!;

    protected List<StepProgress> steps = new();
    protected int completedCount;

    protected override async Task OnInitializedAsync()
    {
        steps = (await ProgressService.GetAllStepsAsync()).ToList();
        completedCount = await ProgressService.GetCompletedCountAsync();
    }

    protected static string PhaseHoverBorder(string id) => id switch
    {
        "phase-1" => "hover:border-purple-400",
        "phase-2" => "hover:border-blue-400",
        "phase-3" => "hover:border-emerald-400",
        _ => "hover:border-gray-300"
    };

    protected static string PhasePillColors(string id) => id switch
    {
        "phase-1" => "bg-purple-50 text-purple-700 border border-purple-200",
        "phase-2" => "bg-blue-50 text-blue-700 border border-blue-200",
        "phase-3" => "bg-emerald-50 text-emerald-700 border border-emerald-200",
        _ => "bg-gray-50 text-gray-700 border border-gray-200"
    };

    protected static string PhaseGradient(string id) => id switch
    {
        "phase-1" => "bg-gradient-to-r from-purple-200 to-purple-400",
        "phase-2" => "bg-gradient-to-r from-blue-200 to-blue-400",
        "phase-3" => "bg-gradient-to-r from-emerald-200 to-emerald-400",
        _ => "bg-gradient-to-r from-gray-200 to-gray-300"
    };
}
