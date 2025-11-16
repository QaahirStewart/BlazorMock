# Admin Step 3 — Build a Role-Aware Dashboard

## Overview

Step 3 turns your claims-based auth into a real-feeling admin dashboard. You use roles and the `amr` claim from the mock auth service to show different content for Regular vs Admin users and to highlight sensitive, admin-only actions.

## Files in This Folder

- `Example.razor` — Tutorial page that explains the role-aware dashboard and embeds the live demo.
- `Example.razor.cs` — Code-behind for the tutorial page, including progress tracking and code-block JS initialization.
- `AdminDashboardExample.razor` — Demo component that renders a SaaS-style dashboard with metrics plus an admin-only “Danger Zone” section.
- `AdminDashboardExample.razor.cs` — Code-behind that injects `IAuthService` and exposes `IsAdmin` and `AuthMethod` helpers.

## Routes

- Tutorial page: `/admin-examples/step3`
- Demo component: `/admin-examples/demo-dashboard`

## What Students Learn

1. How to read roles and auth method from a shared auth service.
2. How to gate admin-only sections using role checks instead of magic booleans.
3. How a dashboard can surface the authentication method (e.g., passkey vs password) for auditing and sensitive flows.
4. How to build a compact, SaaS-inspired admin view in Blazor.

## Key Concepts

- Role-based authorization.
- Claims-driven UI.
- Passkey-aware dashboards (using the `amr` claim).

## Architecture

- The dashboard uses `IAuthService` to get the current `AuthUser` and to evaluate `IsInRole("Admin")`.
- It reads the `amr` claim (`"pwd"` vs `"webauthn"`) from the `ClaimsPrincipal` for display and potential conditional behavior.
- Regular metrics are visible to all authenticated users; the Danger Zone is wrapped in an `IsAdmin` check.
- Progress is tracked via `ILearningProgressService` using the `"admin"` project key and step number `3`.

## Prerequisites

- Admin Step 1 and Step 2 completed.
- The `AdminAuthExample` demo working so you can sign in as Regular or Admin.

## Next Steps

- Connect this mock dashboard to real data in your own projects.
- Replace the in-memory `IAuthService` with ASP.NET Core Identity or an external identity provider that issues similar claims.

## Code Structure

- `AdminDashboardExampleBase` injects `IAuthService` and exposes `CurrentUser`, `IsAuthenticated`, `IsAdmin`, and `AuthMethod` properties.
- The Razor file uses `<if>` / `else` blocks to decide which cards to render based on `IsAdmin`.
- `ExampleBase` mirrors the Step 2 tutorial structure for consistency.

## Styling Patterns

- Uses the same gradient “What You’ll Learn” banner and white cards as other examples.
- Metrics are displayed in pill-style cards; the Danger Zone uses red accents to clearly communicate risk.

## Common Issues & Solutions

- **Dashboard always shows unauthenticated view**: Make sure you signed in via the Step 2 auth demo and that both components use the same scoped `IAuthService` registration.
- **Admin-only section doesn’t appear**: Confirm that your sign-in flow assigns the `"Admin"` role and that the `IsInRole("Admin")` call matches that string exactly.
- **`amr` never changes**: Verify that your auth service sets `"pwd"` for regular sign-ins and `"webauthn"` for passkey-style sign-ins.

## Related Resources

- Official docs: _Authorize view and role-based authorization in ASP.NET Core_.
- Official docs: _Passkeys and WebAuthn support in ASP.NET Core Identity_.
