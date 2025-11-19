using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class ProfileBase : ComponentBase
{
    [Inject] protected IUserAuthService Auth { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected string fullName = string.Empty;
    protected string profilePictureUrl = string.Empty;
    protected string? saveMessage;

    protected override void OnInitialized()
    {
        if (Auth.CurrentUser != null)
        {
            fullName = Auth.CurrentUser.FullName;
            profilePictureUrl = Auth.CurrentUser.ProfilePictureUrl ?? string.Empty;
        }
    }

    protected string GetInitials()
    {
        if (Auth.CurrentUser == null) return "?";
        var parts = Auth.CurrentUser.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2) return $"{parts[0][0]}{parts[1][0]}".ToUpper();
        return Auth.CurrentUser.FullName[0].ToString().ToUpper();
    }

    protected string GetRoleBadgeClass()
    {
        return Auth.CurrentUser?.Role switch
        {
            "Admin" => "bg-red-100 text-red-800",
            "Paid" => "bg-purple-100 text-purple-800",
            _ => "bg-gray-100 text-gray-800"
        };
    }

    protected string GetRoleColorClass()
    {
        return Auth.CurrentUser?.Role switch
        {
            "Admin" => "text-red-600",
            "Paid" => "text-purple-600",
            _ => "text-gray-600"
        };
    }

    protected void HandleSave()
    {
        Auth.UpdateProfile(fullName, string.IsNullOrWhiteSpace(profilePictureUrl) ? null : profilePictureUrl);
        saveMessage = "Profile updated successfully!";
        StateHasChanged();
    }

    protected void UpdateProfilePicture()
    {
        if (!string.IsNullOrWhiteSpace(profilePictureUrl))
        {
            Auth.UpdateProfile(fullName, profilePictureUrl);
            saveMessage = "Profile picture updated!";
            StateHasChanged();
        }
    }

    protected void UpgradeToPaid()
    {
        Auth.UpgradeToRole("Paid");
        saveMessage = "Upgraded to Paid account! 🎉";
        StateHasChanged();
    }

    protected void HandleSignOut()
    {
        Auth.SignOut();
        Nav.NavigateTo("/signin");
    }
}
