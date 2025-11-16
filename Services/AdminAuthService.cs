using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorMock.Services;

public interface IUserAuthService
{
    bool IsAuthenticated { get; }
    UserAccount? CurrentUser { get; }
    List<UserAccount> AllUsers { get; }
    
    event Action? OnChange;

    bool SignUp(string email, string password, string fullName);
    bool SignIn(string email, string password);
    void SignOut();
    void UpdateProfile(string fullName, string? profilePictureUrl);
    void UpgradeToRole(string role);
    UserAccount? GetUserById(int id);
    void AdminUpdateUserRole(int userId, string newRole);
    void AdminDeleteUser(int userId);
}

public class UserAccount
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "Free"; // Free, Paid, Admin
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
}

public sealed class UserAuthService : IUserAuthService
{
    private UserAccount? _currentUser;
    private readonly List<UserAccount> _users = new();
    private int _nextUserId = 1;

    public bool IsAuthenticated => _currentUser != null;
    public UserAccount? CurrentUser => _currentUser;
    public List<UserAccount> AllUsers => _users.ToList();

    public event Action? OnChange;

    public UserAuthService()
    {
        // Seed admin account
        _users.Add(new UserAccount
        {
            Id = _nextUserId++,
            Email = "admin@demo.com",
            PasswordHash = "admin123", // In real app, use proper hashing
            FullName = "System Admin",
            Role = "Admin",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.Now.AddDays(-30),
            LastLoginAt = DateTime.Now
        });

        // Seed sample users
        _users.Add(new UserAccount
        {
            Id = _nextUserId++,
            Email = "user@demo.com",
            PasswordHash = "user123",
            FullName = "Demo User",
            Role = "Free",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.Now.AddDays(-10),
            LastLoginAt = DateTime.Now.AddHours(-2)
        });

        _users.Add(new UserAccount
        {
            Id = _nextUserId++,
            Email = "paid@demo.com",
            PasswordHash = "paid123",
            FullName = "Premium User",
            Role = "Paid",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.Now.AddDays(-5),
            LastLoginAt = DateTime.Now.AddMinutes(-30)
        });
    }

    public bool SignUp(string email, string password, string fullName)
    {
        if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return false;

        var newUser = new UserAccount
        {
            Id = _nextUserId++,
            Email = email,
            PasswordHash = password, // In real app, hash this
            FullName = fullName,
            Role = "Free",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.Now,
            LastLoginAt = DateTime.Now
        };

        _users.Add(newUser);
        _currentUser = newUser;
        NotifyStateChanged();
        return true;
    }

    public bool SignIn(string email, string password)
    {
        var user = _users.FirstOrDefault(u => 
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
            u.PasswordHash == password);

        if (user == null)
            return false;

        user.LastLoginAt = DateTime.Now;
        _currentUser = user;
        NotifyStateChanged();
        return true;
    }

    public void SignOut()
    {
        _currentUser = null;
        NotifyStateChanged();
    }

    public void UpdateProfile(string fullName, string? profilePictureUrl)
    {
        if (_currentUser == null) return;

        _currentUser.FullName = fullName;
        _currentUser.ProfilePictureUrl = profilePictureUrl;
        NotifyStateChanged();
    }

    public void UpgradeToRole(string role)
    {
        if (_currentUser == null) return;
        _currentUser.Role = role;
        NotifyStateChanged();
    }

    public UserAccount? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public void AdminUpdateUserRole(int userId, string newRole)
    {
        if (_currentUser?.Role != "Admin") return;

        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.Role = newRole;
            NotifyStateChanged();
        }
    }

    public void AdminDeleteUser(int userId)
    {
        if (_currentUser?.Role != "Admin") return;

        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _users.Remove(user);
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

// Keep old AdminAuthService for compatibility with existing demo pages
public interface IAdminAuthService
{
    bool IsSignedIn { get; }
    string CurrentRole { get; }
    string DisplayName { get; }
    event Action? OnChange;
    void SignInAsRegular();
    void SignInAsSuperAdmin();
    void SignOut();
}

public sealed class AdminAuthService : IAdminAuthService
{
    private bool _isSignedIn;
    private string _currentRole = "Regular";
    private string _displayName = "Guest";

    public bool IsSignedIn => _isSignedIn;
    public string CurrentRole => _currentRole;
    public string DisplayName => _displayName;
    public event Action? OnChange;

    public void SignInAsRegular()
    {
        _isSignedIn = true;
        _currentRole = "Regular";
        _displayName = "Sarah Anderson";
        NotifyStateChanged();
    }

    public void SignInAsSuperAdmin()
    {
        _isSignedIn = true;
        _currentRole = "SuperAdmin";
        _displayName = "Sarah Anderson";
        NotifyStateChanged();
    }

    public void SignOut()
    {
        _isSignedIn = false;
        _currentRole = "Regular";
        _displayName = "Guest";
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
