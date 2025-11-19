# Admin Dashboard Step 9 — Dashboard Layout Shell (/admin/dashboard)

## Overview

This step builds the first version of the `/admin/dashboard` page: a responsive layout that pulls together key metrics and navigation areas.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the dashboard layout shell.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step9`

## What Students Learn

1. How to create a dedicated `/admin/dashboard` page.
2. How to design a layout with header, sidebar (or nav), and main content areas.
3. How to place summary cards and analytics components inside the dashboard.
4. How to ensure the layout works well on both large and small screens.

## Key Concepts

- Layout composition in Blazor
- Responsive design using Tailwind CSS
- Reusing analytics and profile components on the dashboard
- Separating layout from data queries where possible

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step9` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `9`.
- Leverages analytics data from Steps 6–8.

## Prerequisites

- Steps 0–8 completed.

## Next Steps

- Proceed to Step 10 to lock down access to `/profile`, `/analytics`, and `/admin/dashboard` using route protection.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on layout markup and component composition.

## Common Issues & Solutions

- **Layout breaking on mobile**: Test at smaller breakpoints and adjust flex/grid behavior.
- **Empty dashboard**: Ensure analytics and user data are wired correctly into the layout.

## Related Resources

- Tailwind CSS layout and grid docs
- Blazor layout and component composition
