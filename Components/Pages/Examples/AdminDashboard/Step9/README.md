# Admin Dashboard Step 9 — Real Admin Experience (/admin/dashboard)

## Overview

In Steps 1–8 you bootstrapped a brand-new Blazor Server app, created the `User` entity plus `AppDbContext`, wired up `IUserAuthService`, and shipped `/signup`, `/signin`, `/profile`, and `/analytics`. Step 9 turns that clean project into the real admin dashboard: a guarded `/admin/dashboard` route that surfaces seeded metrics, renders the responsive user-management UI, and calls admin helpers on the same service you already built.

This page no longer references anything from the starter BlazorMock repo—the snippets below are the files you paste into your fresh app.

## Quick Start (Copy/Paste Friendly)

1. **Namespace + `_Imports` check** – Confirm your `.csproj` sets the correct `RootNamespace` (the tutorial uses `AdminDashboard`; this repo uses `BlazorMock`) and update `Components/_Imports.razor` to use `@using global::<YourRootNamespace>...` so the shared directives always target the root (prevents the `AdminDashboard` component class under `AdminDashboard.Components.Pages.Admin` from colliding).
2. **Keep Steps 1–8 in place** – You need the `User` model, `AppDbContext`, analytics samples, and the Step 8 version of `IUserAuthService`/`DemoUserAuthService` already running.
3. **Create the admin page** – Add `Components/Pages/Admin/AdminDashboard.razor` and `AdminDashboard.razor.cs` exactly as shown in the Example. They live in the `AdminDashboard.Components.Pages.Admin` namespace, declare `@page "/admin/dashboard"`, and inherit `AdminDashboardBase` so you can separate markup from logic.
4. **Expand the auth service** – Update `Services/IUserAuthService.cs` to expose `bool IsAdmin`, `IReadOnlyList<User> AllUsers`, `void AdminUpdateUserRole(int userId, string newRole)`, and `void AdminDeleteUser(int userId)`. Paste the matching implementation from the Example into `Services/DemoUserAuthService.cs` so both methods hit the EF-backed `Users` table.
5. **Register everything** – `Program.cs` should already call `builder.Services.AddScoped<IUserAuthService, DemoUserAuthService>();`. No extra DI changes are required, but you do need to add a nav link (or manual URL) to reach `/admin/dashboard`.
6. **Seed accounts** – Run `dotnet ef database update` once more if you reset your SQLite file. Make sure the `admin@demo.com / admin123` account exists or the guard will always show the lockout card.

> 💡 **Copy/paste tip:** All Razor snippets inside the tutorial use doubled `@@` so the docs can render them. After pasting a file into your clean app, run a quick find/replace (`@@` → `@`).

## Files in This Folder

- `Example.razor` — Tutorial page documenting the stand-alone admin dashboard implementation.
- `Example.razor.cs` — Code-behind for tutorial progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step9`
- Student project route: `/admin/dashboard`

## What Students Learn

1. Locking `/admin/dashboard` behind the `IUserAuthService` admin role with a friendly unauthorized card.
2. Surfacing KPI tiles, mobile cards, and desktop tables directly from seeded `Auth.AllUsers` data.
3. Sharing one code-behind helper (`GetRoleSelectClass`) between responsive layouts to keep styling consistent.
4. Calling `Auth.AdminUpdateUserRole` and `Auth.AdminDeleteUser` without reaching into `AppDbContext` from the page.

## Key Concepts

- Role-gated routes backed by dependency-injected services.
- Deterministic seed data that keeps tutorials, screenshots, and QA checks in sync.
- Responsive Tailwind patterns that switch between `md:hidden` card stacks and `hidden md:block` tables.
- Component base classes that encapsulate navigation, styling helpers, and service calls.

## Architecture

- Tutorial page inherits `ExampleBase` within `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step9`.
- Uses `ILearningProgressService` to mark `admin-dashboard` step `9` complete.
- Sample files are authored for the fresh AdminDashboard project and live under `Components/Pages/Admin` plus `Services`.

## Prerequisites

- Steps 0–8 completed (clean project, EF Core, `/signup`, `/signin`, `/profile`, `/analytics`, seeded demo data, and the Step 8 auth service).

## Next Steps

- Step 10 applies the same guard pattern to `/profile`, `/analytics`, and supporting modals.

## Code Structure

- `ExampleBase` continues to handle completion tracking and JS helpers.
- `Example.razor` now focuses on the real admin guard, KPI grid, responsive user lists, and the service updates required to make it work in a blank app.

## Common Issues & Solutions

- **Admin lockout showing for admins**: Ensure `Auth.CurrentUser?.Role == "Admin"` by signing in as `admin@demo.com` or updating the seed data accordingly.
- **Dropdown styles missing**: Call `@@GetRoleSelectClass` in both the mobile select and the desktop table select.
- **Delete button visible on yourself**: Keep the `user.Id == Auth.CurrentUser?.Id` guard so admins cannot delete their own account.
- **`AdminUpdateUserRole` not found**: Update both `IUserAuthService` and `DemoUserAuthService` with the new admin helpers before compiling the page.

## Related Resources

- `Components/Pages/Examples/AdminDashboard/Step8` (analytics polish you build on here)
- Tips page sections on Auth Service and Tailwind layouts
