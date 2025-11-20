# Admin Dashboard Step 9 — Real Admin Experience (/admin/dashboard)

## Overview

Replace the placeholder dashboard shell with the exact admin experience that ships under `Components/Pages/Demo/DashboardDemo/AdminDashboard`. Students mirror the production page: guard the route by role, surface seeded metrics via `IUserAuthService`, and reuse the responsive user-management UI (mobile cards + desktop table) so tutorials and the live demo stay in sync.

## Files in This Folder

- `Example.razor` — Tutorial page documenting the canonical admin dashboard.
- `Example.razor.cs` — Code-behind for tutorial progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step9`

## What Students Learn

1. Blocking non-admin roles from `/admin/dashboard` with the same service used in the demo.
2. Using `Auth.AllUsers` counts to drive KPI tiles without duplicating EF queries.
3. Rendering both `md:hidden` card stacks and `hidden md:block` tables for responsive parity.
4. Wiring role dropdowns and delete buttons into `Auth.AdminUpdateUserRole` / `Auth.AdminDeleteUser` helpers.

## Key Concepts

- Role-gated routes backed by `IUserAuthService`.
- Deterministic, seeded sample data for docs + QA parity.
- Responsive Tailwind patterns that swap card and table layouts.
- Code-behind helpers that centralize styling (`GetRoleSelectClass`) and service calls.

## Architecture

- Tutorial page inherits `ExampleBase` within `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step9`.
- Uses `ILearningProgressService` to mark `admin-dashboard` step `9` complete.
- Samples are HTML-encoded copies of `DashboardDemo/AdminDashboard.razor` and `.razor.cs`.

## Prerequisites

- Steps 0–8 completed (auth service + analytics tiles already in place).

## Next Steps

- Step 10 applies the same auth gating to `/profile`, `/analytics`, and other routes.

## Code Structure

- `ExampleBase` continues to handle completion tracking and JS helpers.
- `Example.razor` focuses on the admin guard, KPI grid, responsive user lists, and code-behind snippets.

## Common Issues & Solutions

- **Admin lockout showing for admins**: Ensure your seeded admin still has `Role == "Admin"`.
- **Dropdown styles missing**: Apply `@@GetRoleSelectClass` to both mobile and desktop selects.
- **Delete button visible on yourself**: Preserve the `user.Id == Auth.CurrentUser.Id` guard.

## Related Resources

- `Components/Pages/Demo/DashboardDemo/AdminDashboard.razor`
- Tips page sections on Auth Service and Tailwind layouts
