using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Admin.Step2;

public partial class AdminStatCardBase : ComponentBase
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public string HelperText { get; set; } = string.Empty;
    [Parameter] public string Status { get; set; } = "neutral";

    protected string GetStatusTextClass() => Status switch
    {
        "good" => "text-emerald-600",
        "warning" => "text-amber-600",
        _ => "text-slate-600"
    };
}
