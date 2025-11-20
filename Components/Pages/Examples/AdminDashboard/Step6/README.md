# Admin Dashboard Step 6 — Basic Analytics Model

## Overview

This step introduces a simple analytics model so the app can record key user actions for later visualization.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the analytics entities and wiring.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step6`

## What Students Learn

1. How to define a simple `Activity` or `Event` entity (e.g., `UserLogin`, `ProfileUpdated`).
2. How to add the analytics entity to `AppDbContext` and run a new migration.
3. How to write analytics events when users perform actions (login, profile updates, dashboard views).
4. How to design analytics data that is easy to query.

## Key Concepts

- Designing simple event/analytics tables
- EF Core migrations for evolving schema
- Recording events from key user actions
- Queryability versus over-normalization

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step6` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `6`.
- Extends the existing `AppDbContext` from previous steps.

## Prerequisites

- Steps 0–5 completed so the app has users and a profile flow.

## Next Steps

- Proceed to Step 7 to start displaying analytics data in `/analytics` using tables and summary counts.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on the new analytics entity and where to write events.

## Common Issues & Solutions

- **Migration issues**: Ensure the previous migrations have been applied and the database is up-to-date.
- **Too many events**: Add simple filters or throttling if needed for demo purposes.

## Related Resources

- EF Core migrations and schema evolution
- Basic analytics/event modeling patterns
