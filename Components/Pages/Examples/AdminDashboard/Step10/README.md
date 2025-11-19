# Admin Dashboard Step 10 — Route Protection & Roles (Canonical)

## Overview

Mirror the live demo’s guard patterns: `/profile` and `/analytics` immediately redirect anonymous visitors to `/signin`, free users see the deterministic upgrade card from `Analytics.razor`, and only admins get the red KPI strip plus the full `/admin/dashboard` shell.

## Files in This Folder

- `Example.razor` — Documentation page with HTML-encoded snippets from the real Profile, Analytics, and AdminDashboard components.
- `Example.razor.cs` — Progress-tracking code-behind (unchanged from earlier steps).

## Routes

- Tutorial page: `/admin-dashboard-examples/step10`

## What Students Learn

1. How to use `IUserAuthService` (`Auth.CurrentUser`) to block signed-out users before any layout renders.
2. How `AnalyticsBase` exposes `IsPaidOrAdmin` and `IsAdmin` helpers for feature gating.
3. How the upgrade CTA and admin-only strip are wired to real navigation targets (back to `/profile` or `/signin`).
4. How to test every persona using the seeded `demo=` links that pre-fill `/signin` with a redirect.

## Key Concepts

- Deterministic redirects via `NavigationManager`.
- Friendly limited-access messaging pulled from production components.
- Role-based branching that reuses service data instead of new EF queries.
- Documentation snippets that stay in sync with `Components/Pages/Demo/DashboardDemo/*`.

## Architecture

- `Example.razor` inherits `ExampleBase` (for checklists + JS helpers) and only renders documentation content.
- Real guards live in `Profile.razor`, `Analytics.razor`, `AdminDashboard.razor`, and they all inject `IUserAuthService` through their partial base classes.
- Sign-in now understands `?demo=role&redirect=/path`, which the Live Demo section links to for quick testing.

## Prerequisites

- Steps 0–9 completed (auth service, seeded users, admin shell).

## Next Steps

- Step 11 focuses on navigation polish across the same routes once guards are aligned.

## Code Structure

- `ExampleBase` keeps tracking/JS behavior.
- `Example.razor` hosts the encoded snippets + live-demo links; no additional services are injected here.

## Common Issues & Solutions

- **Still seeing profile content when logged out**: ensure the guard stays at the very top of `Profile.razor`.
- **Paid users missing advanced analytics**: confirm `IsPaidOrAdmin` checks for both `"Paid"` and `"Admin"` (case-sensitive).
- **Admin lockout text differs**: copy the exact markup from `AdminDashboard.razor` so docs and demo stay identical.

## Related Resources

- `Components/Pages/Demo/DashboardDemo/Profile.razor`
- `Components/Pages/Demo/DashboardDemo/Analytics.razor`
- `Components/Pages/Demo/DashboardDemo/AdminDashboard.razor`
