# Admin Dashboard Step 7 — Analytics Page (/analytics): Paid Feature Gating

## Overview

Step 7 syncs the tutorial with the real `/analytics` experience from `DashboardDemo`. Learners swap the EF-driven table mockups for the production markup that shows baseline metrics to everyone, premium cards + traffic sources to paid accounts, and an admin-only totals grid.

## Files in This Folder

- `Example.razor` — Guided walkthrough of the production analytics sections, including the locked upgrade CTA for Free users.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step7`

## What Students Learn

1. How `AnalyticsBase` and `IUserAuthService` work together to redirect anonymous visitors and expose helper booleans.
2. How to wrap premium UI in `IsPaidOrAdmin` checks and present a graceful locked state for Free plans.
3. How to reuse the deterministic `trafficSources` list so screenshots always match the shipped demo.
4. How to surface admin-only totals from `Auth.AllUsers` without duplicating EF queries.

## Key Concepts

- Auth-driven feature gating
- Deterministic sample data for documentation
- Tailwind layouts for premium cards, traffic lists, and alert CTAs
- Service-powered admin summaries

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step7` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `7`.
- References the same markup + helpers that live in `Components/Pages/Demo/DashboardDemo/Analytics.razor`.

## Prerequisites

- Steps 0–6 completed (including the baseline `/analytics` scaffolding).
- `IUserAuthService` wired up so Free, Paid, and Admin test accounts exist.

## Next Steps

- Proceed to Step 8 to explain how the same page uses CSS-only micro-visuals (bars, progress indicators) without chart libraries.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page highlights markup excerpts for premium cards, traffic sources, and admin totals, plus the `AnalyticsBase` helper class.

## Common Issues & Solutions

- **Paid UI shows for Free users**: Confirm you updated the markup to wrap premium sections in `@if (IsPaidOrAdmin)`.
- **Locked CTA never appears**: Make sure Free testers sign in via `/signin` and that the upgrade card lives in the `else` branch.
- **Admin grid empty**: The seeded demo accounts must include at least one Admin so `Auth.AllUsers` contains role diversity.

## Related Resources

- `Components/Pages/Demo/DashboardDemo/Analytics.razor`
- `Components/Pages/Demo/DashboardDemo/Analytics.razor.cs`
- `Services/IUserAuthService`
