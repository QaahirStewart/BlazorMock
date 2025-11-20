# Admin Dashboard Step 2 — Minimal Auth Model & DbContext

## Overview

This step introduces a teaching-only `AppDbContext` and `User` entity so students can persist users for the Admin Dashboard example.

## Quick Start (Copy/Paste Friendly)

Following these five moves keeps a brand-new `dotnet new blazorserver` app from throwing namespace or DbContext errors when you paste the snippets:

1. **Align the namespace** – The Blazor template sets the namespace to your project name. Either rename it to `AdminDashboard` in the `.csproj`, or replace every `AdminDashboard.*` namespace in the snippets with your real root namespace (e.g., `MyDashboardApp`). Most copy/paste build errors (`CS0246`) happen because this step gets skipped.
2. **Install EF Core packages once**
   ```pwsh
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   dotnet tool install --global dotnet-ef
   ```
   SQLite keeps the tutorial self-contained; swap to `SqlServer` only if you already have SQL Server running locally.
3. **Create the two files** – Add `Models/User.cs` and `Data/AppDbContext.cs` exactly as shown further down, keeping the namespace from step 1.
4. **Register the DbContext in `Program.cs`** – Drop in the snippet from this page. It falls back to an on-disk SQLite file if the connection string is missing, which removes another common pitfall.
5. **Add the first migration**
   ```pwsh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   If the command fails, re-run `dotnet build` to ensure the project compiles before running migrations.

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

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step2` namespace.
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

- **`CS0246` namespace errors**: Replace every `AdminDashboard` namespace in the snippets with your project's namespace, or set the `&lt;RootNamespace&gt;` in the `.csproj` to `AdminDashboard`.
- **`CS0234` / `CS0246` EF Core errors**: Run the package installs from the Quick Start (`dotnet add package Microsoft.EntityFrameworkCore`, `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`, `dotnet add package Microsoft.EntityFrameworkCore.Tools`, and `dotnet tool install --global dotnet-ef`). These errors mean the EF assemblies are missing.
- **`CS0246` namespace errors**: Replace every `AdminDashboard` namespace in the snippets with your project's namespace, or set the `&lt;RootNamespace&gt;` in the `.csproj` to `AdminDashboard`.
- **Provider not configured**: Install the matching provider package (the guide uses `Microsoft.EntityFrameworkCore.Sqlite`) and call `UseSqlite` or `UseSqlServer` consistently.
- **Connection string errors**: Double-check the database path/name and provider configuration; the sample falls back to `Data Source=AdminDashboard.db` if the setting is missing.
- **Migration failures**: Ensure EF Core tools are installed and the project builds before running migrations.
- **Database not created**: Confirm the migration command ran and the app is pointing at the correct database.

## Related Resources

- EF Core docs (getting started)
- Connection strings configuration in ASP.NET Core
