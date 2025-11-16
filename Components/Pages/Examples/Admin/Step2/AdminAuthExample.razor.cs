using System.Linq;
using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Admin.Step2;

public partial class AdminAuthExampleBase : ComponentBase
{
    [Inject] protected IAuthService AuthService { get; set; } = default!;

    protected AuthUser? CurrentUser => AuthService.CurrentUser;
    protected bool IsAuthenticated => AuthService.IsAuthenticated;

    protected string AuthMethod => AuthService.Principal?.Claims.FirstOrDefault(c => c.Type == "amr")?.Value ?? "none";
    protected string DeviceName => AuthService.Principal?.Claims.FirstOrDefault(c => c.Type == "device")?.Value ?? "(none)";

    protected void SignInRegular()
    {
        AuthService.SignInAsRegular();
        StateHasChanged();
    }

    protected void SignInAdmin()
    {
        AuthService.SignInAsAdmin();
        StateHasChanged();
    }

    protected void SignInWithPasskey()
    {
        // In a real .NET 10 app, this would be driven by WebAuthn
        AuthService.SignInWithPasskey("Dev Laptop", asAdmin: true);
        StateHasChanged();
    }

    protected void SignOut()
    {
        AuthService.SignOut();
        StateHasChanged();
    }
}
