using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Auth;

public partial class SignInBase : ComponentBase
{
    [Inject] protected IUserAuthService Auth { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected string email = string.Empty;
    protected string password = string.Empty;
    protected string errorMessage = string.Empty;
    protected bool isPasskeyAuthenticating = false;
    protected string passkeyEmail = "admin@demo.com";
    protected string passkeyPassword = "admin123";

    protected void HandleSignIn()
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Please enter both email and password.";
            return;
        }

        if (Auth.SignIn(email, password))
        {
            Nav.NavigateTo("/profile");
        }
        else
        {
            errorMessage = "Invalid email or password.";
        }
    }

    protected async Task HandlePasskeySignIn()
    {
        errorMessage = string.Empty;
        isPasskeyAuthenticating = true;
        StateHasChanged();

        // Simulate passkey authentication delay
        await Task.Delay(1500);

        // Use the selected passkey user
        if (Auth.SignIn(passkeyEmail, passkeyPassword))
        {
            Nav.NavigateTo("/profile");
        }
        else
        {
            errorMessage = "Passkey authentication failed.";
            isPasskeyAuthenticating = false;
            StateHasChanged();
        }
    }

    protected void SetDemoUser(string demoEmail, string demoPassword)
    {
        email = demoEmail;
        password = demoPassword;
        passkeyEmail = demoEmail;
        passkeyPassword = demoPassword;
        errorMessage = string.Empty;
        StateHasChanged();
    }
}
