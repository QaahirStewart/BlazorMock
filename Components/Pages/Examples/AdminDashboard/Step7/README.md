# Admin Dashboard Step 7 — Layer Paid + Admin Sections (/analytics)

## Overview

Extend the Step 6 baseline. Replace the dashed placeholder inside `Analytics.razor` with the real paid-only cards, traffic source list, and locked upgrade CTA. Then append the admin summary grid beneath the KPI stack. Everything stays powered by `IUserAuthService`, so you only touch markup and the shared `AnalyticsBase` helpers.

## Files in This Folder

- `Example.razor` — Guided walkthrough of the production analytics sections, including the locked upgrade CTA for Free users.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step7`

## What Students Learn

1. How to evolve the Step 6 page by swapping a placeholder block for the paid/locked layout without re-copying the whole component.
2. How `AnalyticsBase` exposes `IsPaidOrAdmin`/`IsAdmin` so markup can branch between premium content and upgrade CTAs.
3. How to reuse the deterministic `trafficSources` list so screenshots always match the shipped demo.
4. How to surface admin-only totals from `Auth.AllUsers` while keeping EF out of the component.

## Key Concepts

- Auth-driven feature gating
- Deterministic sample data for documentation
- Tailwind layouts for premium cards, traffic lists, and alert CTAs
- Service-powered admin summaries

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step7` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `7`.
- Reuses the Step 6 layout (guard + hero + KPI cards) and only adds the premium/admin fragments highlighted below.

## Prerequisites

- Steps 0–6 completed (including the baseline `/analytics` scaffolding).
- `IUserAuthService` wired up so Free, Paid, and Admin test accounts exist.

## Next Steps

- Proceed to Step 8 to polish the visuals (micro-bars, gradients, responsive tweaks) without reworking the logic.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page highlights the exact sections you paste: the paid/locked block that replaces the Step 6 placeholder, the admin summary grid appended below the KPIs, and the helper members you add to `AnalyticsBase`.

## Common Issues & Solutions

- **Paid UI shows for Free users**: Confirm you updated the markup to wrap premium sections in `@if (IsPaidOrAdmin)`.
- **Locked CTA never appears**: Make sure Free testers sign in via `/signin` and that the upgrade card lives in the `else` branch.
- **Admin grid empty**: The seeded demo accounts must include at least one Admin so `Auth.AllUsers` contains role diversity.

## Related Resources

- `Components/Pages/Demo/DashboardDemo/Analytics.razor`
- `Components/Pages/Demo/DashboardDemo/Analytics.razor.cs`
- `Services/IUserAuthService`
