using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
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
    protected string redirectDestination = "/profile";
    private bool _hasRendered;

    protected override void OnInitialized()
    {
        ApplyQueryState();
    }

    protected override void OnParametersSet()
    {
        ApplyQueryState();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _hasRendered = true;
        }
    }

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
            Nav.NavigateTo(redirectDestination);
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
            Nav.NavigateTo(redirectDestination);
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
        if (_hasRendered)
        {
            _ = InvokeAsync(StateHasChanged);
        }
    }

    private void ApplyQueryState()
    {
        var uri = Nav.ToAbsoluteUri(Nav.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        if (query.TryGetValue("redirect", out var redirectValues))
        {
            var candidate = redirectValues.ToString();
            if (!string.IsNullOrWhiteSpace(candidate) && candidate.StartsWith("/"))
            {
                redirectDestination = candidate;
            }
            else
            {
                redirectDestination = "/profile";
            }
        }
        else
        {
            redirectDestination = "/profile";
        }

        if (query.TryGetValue("demo", out var demoValues))
        {
            var role = demoValues.ToString().ToLowerInvariant();
            switch (role)
            {
                case "admin":
                    SetDemoUser("admin@demo.com", "admin123");
                    break;
                case "paid":
                    SetDemoUser("paid@demo.com", "paid123");
                    break;
                case "free":
                    SetDemoUser("user@demo.com", "user123");
                    break;
            }
        }
    }
}
