using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class AnalyticsBase : ComponentBase
{
    [Inject] protected IUserAuthService Auth { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected bool IsPaidOrAdmin => Auth.CurrentUser?.Role == "Paid" || Auth.CurrentUser?.Role == "Admin";
    protected bool IsAdmin => Auth.CurrentUser?.Role == "Admin";

    protected List<TrafficSource> trafficSources = new()
    {
        new TrafficSource { Icon = "🔍", Name = "Google Search", Visitors = 1234, Percentage = 42 },
        new TrafficSource { Icon = "📱", Name = "Social Media", Visitors = 892, Percentage = 30 },
        new TrafficSource { Icon = "📧", Name = "Email Campaign", Visitors = 567, Percentage = 19 },
        new TrafficSource { Icon = "🔗", Name = "Direct", Visitors = 234, Percentage = 9 }
    };

    protected string GetRoleBadgeClass()
    {
        return Auth.CurrentUser?.Role switch
        {
            "Admin" => "bg-red-100 text-red-800",
            "Paid" => "bg-purple-100 text-purple-800",
            _ => "bg-gray-100 text-gray-800"
        };
    }

    protected class TrafficSource
    {
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Visitors { get; set; }
        public int Percentage { get; set; }
    }
}
