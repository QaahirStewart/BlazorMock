using System.Linq;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Admin.Step3;

public partial class AdminDashboardExampleBase : ComponentBase
{
    [Inject] protected IAuthService AuthService { get; set; } = default!;

    protected AuthUser? CurrentUser => AuthService.CurrentUser;
    protected bool IsAuthenticated => AuthService.IsAuthenticated;
    protected bool IsAdmin => AuthService.IsInRole("Admin");

    protected string AuthMethod => AuthService.Principal?.Claims.FirstOrDefault(c => c.Type == "amr")?.Value ?? "none";
}
