# Admin Dashboard Step 11 — Desktop Navigation

## Overview

This step adds a desktop navigation experience that surfaces the main admin routes and makes it easy to move between pages.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the desktop navigation design.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step11`

## What Students Learn

1. How to build a top or side navigation bar for desktop.
2. How to include links to `/signin`, `/signup`, `/profile`, `/analytics`, and `/admin/dashboard`.
3. How to highlight the active route in the nav.
4. How to hide or show certain nav items when the user is signed in/out.

## Key Concepts

- Navigation patterns in Blazor
- Styling active nav links
- Conditional rendering based on auth state

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step11` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `11`.
- Works together with route protection from Step 10.

## Prerequisites

- Steps 0–10 completed.

## Next Steps

- Proceed to Step 12 to create a mobile navigation pattern and apply final polish to the experience.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on the desktop nav layout and behavior.

## Common Issues & Solutions

- **Active state not updating**: Ensure you are binding to the current route and refreshing correctly.
- **Links to protected pages failing**: Confirm route protection logic and redirects.

## Related Resources

- Blazor navigation and `NavLink`
- Responsive design patterns for navigation
