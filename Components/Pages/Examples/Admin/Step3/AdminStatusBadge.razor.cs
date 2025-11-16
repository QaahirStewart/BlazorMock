using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Admin.Step3;

public partial class AdminStatusBadgeBase : ComponentBase
{
    [Parameter] public string Status { get; set; } = "active";

    protected string Label => Status switch
    {
        "trial" => "Trial",
        "overdue" => "Overdue",
        _ => "Active"
    };

    protected string BackgroundClass => Status switch
    {
        "trial" => "bg-amber-100 text-amber-800",
        "overdue" => "bg-red-100 text-red-800",
        _ => "bg-emerald-100 text-emerald-800"
    };

    protected string DotClass => Status switch
    {
        "trial" => "bg-amber-500",
        "overdue" => "bg-red-500",
        _ => "bg-emerald-500"
    };
}
