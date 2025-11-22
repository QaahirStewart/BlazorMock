# Admin Dashboard Step 6 — Analytics Foundations (/analytics)

## Overview

Stand up the first version of `/analytics`: a session-restoring guard plus the baseline metrics every plan can see. You copy the hero, skeleton state, and four core KPI cards from the production demo so later steps can layer paid-only visuals on top.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the analytics entities and wiring.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step6`

## What Students Learn

1. Restoring sessions on `/analytics` using `localStorage` + `Auth.SignInByEmail` before redirecting to `/signin`.
2. Rendering the shared hero + four core KPI cards that every plan can access.
3. Keeping the component service-driven (reads `IUserAuthService` only) so later steps toggle UI without touching EF.
4. Preparing placeholders for paid/admin sections that Steps 7–8 will append.

## Key Concepts

- Session restoration in Blazor Server
- Auth-aware page guards
- Deterministic sample data for tutorials
- Incremental component layering

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step6` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `6`.
- Extends the existing `AppDbContext` from previous steps.

## Prerequisites

- Steps 0–5 completed so the app has users and a profile flow.

## Next Steps

- Proceed to Step 7 to add paid gating, the traffic sources list, and the admin-only summary grid.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on the baseline `/analytics` markup and the guard logic.

## Common Issues & Solutions

- **Blank screen**: Confirm the session restore block runs before redirecting—`AnalyticsBase` should only navigate once it fails to find a stored email.
- **Paid cards already visible**: That content now belongs to Step 7. Keep only the hero + four KPIs in this step.
- **Auth service null**: Make sure `IUserAuthService` is registered (from Step 5) and seeded demo accounts still exist.

## Related Resources

- `Components/Pages/Demo/DashboardDemo/Analytics.razor`
- Step 7 and Step 8 example pages for the additive sections
