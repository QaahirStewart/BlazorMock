using Microsoft.AspNetCore.Components;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Demo.DashboardDemo;

public partial class AdminDashboard
{
    [Inject] private IUserAuthService Auth { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    private string GetRoleSelectClass(string role)
    {
        return role switch
        {
            "Admin" => "bg-gray-100 text-red-700 border border-gray-300",
            "Paid" => "bg-gray-100 text-purple-700 border border-gray-300",
            _ => "bg-gray-100 text-gray-800 border border-gray-300"
        };
    }

    private void UpdateUserRole(int userId, string newRole)
    {
        Auth.AdminUpdateUserRole(userId, newRole);
        StateHasChanged();
    }

    private void DeleteUser(int userId)
    {
        Auth.AdminDeleteUser(userId);
        StateHasChanged();
    }
}
