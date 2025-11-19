# Admin Dashboard Step 10 — Route Protection & Roles

## Overview

This step adds route protection so only signed-in users (and optionally users with specific roles) can access `/profile`, `/analytics`, and `/admin/dashboard`.

## Files in This Folder

- `Example.razor` — Tutorial page explaining route protection and role checks.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step10`

## What Students Learn

1. How to protect routes so only authenticated users can access certain pages.
2. How to redirect unauthenticated users to `/signin`.
3. How to check user roles and hide or disable sensitive areas for non-admins.
4. How to surface clear messaging when access is denied.

## Key Concepts

- Authorization checks in Blazor Server
- Redirecting or rendering alternate content for unauthorized users
- Role-based access control (RBAC) at the page and component level

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step10` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `10`.
- Builds on the authentication model from earlier steps.

## Prerequisites

- Steps 0–9 completed.

## Next Steps

- Proceed to Step 11 to build a desktop navigation pattern across `/signin`, `/signup`, `/profile`, `/analytics`, and `/admin/dashboard`.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on authorization patterns and route protection.

## Common Issues & Solutions

- **Users stuck in redirect loops**: Make sure the signin page itself is not protected.
- **Admins treated as regular users**: Double-check role checks and any seed data for admin accounts.

## Related Resources

- ASP.NET Core authorization & roles
- Blazor route/view authorization patterns
