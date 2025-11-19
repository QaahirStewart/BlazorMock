# Admin Dashboard Step 7 — Analytics Page (/analytics): Counters & Tables

## Overview

This step builds the first version of the `/analytics` page, focusing on summary counts and tables powered by the analytics events.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the `/analytics` counters and tables.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step7`

## What Students Learn

1. How to query analytics events to compute summary metrics (e.g., total users, logins today).
2. How to display metrics in simple cards.
3. How to show recent activity in a table.
4. How to keep queries efficient and readable.

## Key Concepts

- EF Core querying for aggregates
- Translating analytics data into UI metrics
- Designing simple metrics cards and tables
- Handling empty states when no data exists yet

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step7` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `7`.
- Uses the analytics model introduced in Step 6.

## Prerequisites

- Steps 0–6 completed.
- Activity events are being recorded for logins/profile updates.

## Next Steps

- Proceed to Step 8 to add CSS-only visualizations (bars and heatmaps) to the `/analytics` page.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on queries, metrics cards, and tables.

## Common Issues & Solutions

- **No data showing**: Confirm events are being written and the queries point to the correct date ranges.
- **Slow queries**: Simplify filters and avoid unnecessary client-side processing.

## Related Resources

- EF Core LINQ aggregation
- UX patterns for analytics dashboards
