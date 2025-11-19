# Admin Dashboard Step 2 — Minimal Auth Model & DbContext

## Overview

This step introduces a teaching-only `AppDbContext` and `User` entity so students can persist users for the Admin Dashboard example.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the auth model and DbContext setup.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step2`

## What Students Learn

1. How to create a simple `User` entity suitable for the tutorial.
2. How to define an `AppDbContext` that includes a `DbSet<User>`.
3. How to configure EF Core (connection string, provider) in a Blazor Server app.
4. How to apply the first migration and create the database.

## Key Concepts

- EF Core DbContext patterns
- `DbSet<TEntity>` for users
- Connection strings in `appsettings.json`
- Migrations and database creation

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step2` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `2`.
- Builds on the clean project and Tailwind setup from Step 1.

## Prerequisites

- Step 0 (Phase 1 guide): prerequisites & VS Code setup.
- Step 1: new clean project and Tailwind v4 wired up.

## Next Steps

- Proceed to Step 3 to build the `/signup` page using this auth model.

## Code Structure

- `ExampleBase` handles:
  - Loading completion state from `ILearningProgressService`.
  - Marking the step complete/incomplete.
  - Initializing JS module for code block enhancements.

## Common Issues & Solutions

- **Connection string errors**: Double-check the database path/name and provider configuration.
- **Migration failures**: Ensure EF Core tools are installed and the project builds before running migrations.
- **Database not created**: Confirm the migration command ran and the app is pointing at the correct database.

## Related Resources

- EF Core docs (getting started)
- Connection strings configuration in ASP.NET Core
