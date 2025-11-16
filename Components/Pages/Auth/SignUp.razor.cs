using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Auth;

public partial class SignUpBase : ComponentBase
{
    [Inject] protected IUserAuthService Auth { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected string fullName = string.Empty;
    protected string email = string.Empty;
    protected string password = string.Empty;
    protected string errorMessage = string.Empty;

    protected void HandleSignUp()
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Please fill in all fields.";
            return;
        }

        if (Auth.SignUp(email, password, fullName))
        {
            Nav.NavigateTo("/profile");
        }
        else
        {
            errorMessage = "An account with this email already exists.";
        }
    }
}
