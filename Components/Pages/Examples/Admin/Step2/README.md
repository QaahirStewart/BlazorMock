# Admin Step 2 &mdash; Wire Up Modern Auth (Mock)

## Overview

Step 2 connects your Admin layout to a simple, claims-based auth service. You learn how to sign in as Regular vs Admin users, simulate a passkey-style sign-in, and inspect the resulting claims (roles, `amr`, device) exactly like a real .NET 10 Identity app would.

## Files in This Folder

- `Example.razor` &mdash; Tutorial page that explains the auth flow and embeds the live demo.
- `Example.razor.cs` &mdash; Code-behind for the tutorial page, including progress tracking and code-block JS initialization.
- `AdminAuthExample.razor` &mdash; Demo component that renders buttons for Regular/Admin/passkey-style sign-in and shows the resulting claims.
- `AdminAuthExample.razor.cs` &mdash; Code-behind that injects `IAuthService` and calls sign-in/out methods.

## Routes

- Tutorial page: `/admin-examples/step2`
- Demo component: `/admin-examples/demo-auth`

## What Students Learn

1. How to inject and use a shared auth service in Blazor components.
2. How to represent users with roles and `ClaimsPrincipal` instead of loose booleans.
3. How `amr` (authentication method reference) and a device claim can distinguish password vs passkey-style flows.
4. How to prepare for role-based UI decisions that power the dashboard in Step 3.

## Key Concepts

- Claims-based identity.
- Role-based authorization.
- Passkey-style authentication via `amr` and device claims.

## Architecture

- The demo uses a small `IAuthService` abstraction with an `InMemoryAuthService` implementation.
- The service returns an `AuthUser` containing username, roles, and a `ClaimsPrincipal`.
- The demo reads claims directly from `AuthService.Principal` and projects them into UI-friendly properties.
- Progress is tracked via `ILearningProgressService` using the `"admin"` project key and step number `2`.

## Prerequisites

- Admin Step 1 completed (layout shell created and working).
- Services registered in `Program.cs`, including `IAuthService` and `ILearningProgressService`.

## Next Steps

- Move on to **Step 3** to plug these roles and claims into the Admin dashboard layout and hide/show admin-only sections.

## Code Structure

- `AdminAuthExampleBase` injects `IAuthService` and exposes helper properties for `CurrentUser`, `IsAuthenticated`, `AuthMethod`, and `DeviceName`.
- Handlers like `SignInAsRegular`, `SignInAsAdmin`, and `SignInWithPasskey` call the service and then `StateHasChanged()`.
- `ExampleBase` follows the shared tutorial code-behind pattern from `TUTORIAL_TEMPLATE.md`.

## Styling Patterns

- Uses the standard tutorial layout: gradient "What You'll Learn" banner, white cards for examples, and a green completion card.
- Buttons use the shared styles from the template: blue for primary actions, green for the passkey-style flow, and subtle borders for secondary actions.

## Common Issues & Solutions

- **UI doesn&apos;t refresh after sign-in**: Ensure each handler calls `StateHasChanged()` after updating auth state.
- **Claims don&apos;t show `amr` or device**: Verify `CreateUser` in `AuthService` adds `amr` and `device` claims and that you&apos;re reading the correct `Type` values (`"amr"` and `"device"`).
- **Progress not marking complete**: Confirm the project key and step number in `ExampleBase` (`"admin"`, `2`) match your `ILearningProgressService` setup.

## Related Resources

- Official docs: _What&apos;s new in ASP.NET Core in .NET 10_ (section: Web Authentication API (passkey) support for ASP.NET Core Identity).
- Official docs: _Implement passkeys in ASP.NET Core Blazor Web Apps_.
