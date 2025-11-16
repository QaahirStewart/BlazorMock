namespace BlazorMock.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public record AuthUser(string UserName, IReadOnlyList<string> Roles, ClaimsPrincipal Principal)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}

public interface IAuthService
{
    AuthUser? CurrentUser { get; }
    bool IsAuthenticated { get; }
    ClaimsPrincipal? Principal { get; }
    bool IsInRole(string role);
    AuthUser SignInAsRegular();
    AuthUser SignInAsAdmin();
    AuthUser SignInWithPasskey(string deviceName, bool asAdmin = false);
    void SignOut();
}

public class InMemoryAuthService : IAuthService
{
    private AuthUser? _currentUser;

    public AuthUser? CurrentUser => _currentUser;

    public bool IsAuthenticated => _currentUser is not null;

    public ClaimsPrincipal? Principal => _currentUser?.Principal;

    public bool IsInRole(string role) => _currentUser?.IsInRole(role) == true;

    public AuthUser SignInAsRegular()
    {
        _currentUser = CreateUser("regular-user", new[] { "Regular" }, authenticationType: "pwd");
        return _currentUser;
    }

    public AuthUser SignInAsAdmin()
    {
        _currentUser = CreateUser("admin-user", new[] { "Regular", "Admin" }, authenticationType: "pwd");
        return _currentUser;
    }

    public AuthUser SignInWithPasskey(string deviceName, bool asAdmin = false)
    {
        var userName = asAdmin ? "admin-passkey" : "regular-passkey";
        var roles = asAdmin ? new[] { "Regular", "Admin" } : new[] { "Regular" };

        // "webauthn" mirrors how real systems tag passkey auth in the amr claim
        _currentUser = CreateUser(userName, roles, authenticationType: "webauthn", deviceName: deviceName);
        return _currentUser;
    }

    public void SignOut()
    {
        _currentUser = null;
    }

    private static AuthUser CreateUser(string userName, IReadOnlyList<string> roles, string authenticationType = "Mock", string? deviceName = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new("amr", authenticationType) // authentication method reference
        };

        if (!string.IsNullOrWhiteSpace(deviceName))
        {
            claims.Add(new Claim("device", deviceName));
        }

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var identity = new ClaimsIdentity(claims, authenticationType);
        var principal = new ClaimsPrincipal(identity);

        return new AuthUser(userName, roles, principal);
    }
}
