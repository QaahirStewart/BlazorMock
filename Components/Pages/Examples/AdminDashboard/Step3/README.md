# Admin Dashboard Step 3 — Sign Up Page (/signup)

## Overview

This step guides students through building a simple `/signup` page so new users can register for the Admin Dashboard.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the `/signup` form and flow.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step3`

## What Students Learn

1. How to build a registration form with fields like email, password, display name, and role.
2. How to validate form input on the server.
3. How to create a new `User` record via EF Core and save it to the database.
4. How to show basic success and error messages.

## Key Concepts

- Blazor forms and validation
- Model binding and validation messages
- Creating new entities with EF Core
- Simple role selection (e.g., Admin vs. Viewer)

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step3` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `3`.
- Uses the `AppDbContext` and `User` entity introduced in Step 2.

## Prerequisites

- Step 0: prerequisites & VS Code setup.
- Step 1: clean project + Tailwind.
- Step 2: minimal auth model and DbContext with migrations applied.

## Next Steps

- Proceed to Step 4 to implement the `/signin` page and login flow.

## Code Structure

- `ExampleBase` handles completion tracking and code block enhancements.
- The tutorial page focuses on the `/signup` UI and server-side logic.

## Common Issues & Solutions

- **Validation not triggering**: Ensure `EditForm`, `DataAnnotationsValidator`, and `ValidationSummary` are correctly configured.
- **Users not saving**: Check that `SaveChangesAsync` is called and no exceptions are being swallowed.
- **Duplicate emails**: Add a uniqueness constraint or manual check to avoid duplicates.

## Related Resources

- Blazor forms and validation docs
- EF Core basics for creating entities
