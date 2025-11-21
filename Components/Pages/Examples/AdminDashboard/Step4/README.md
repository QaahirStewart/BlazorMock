# Admin Dashboard Step 4 — Sign In Page (/signin)

## Overview

This step walks students through building a `/signin` page that lets existing users log into the Admin Dashboard.

## Quick Start (Copy/Paste Friendly)

1. **Match your namespace** – Replace `AdminDashboard.*` with your project's root namespace (or set `&lt;RootNamespace&gt;YourApp&lt;/RootNamespace&gt;` in the `.csproj`).
2. **Have Steps 2 & 3 ready** – You need the `User` model, `AppDbContext`, EF Core packages, and the `/signup` flow so accounts exist to log in with.
3. **Update `IUserAuthService`** – Add a `SignIn(string email, string password)` method to the interface (or copy the full snippet from this step) so both `/signup` and `/signin` compile.
4. **Update the service implementation** – Ensure your `DemoUserAuthService` (or equivalent) implements both `SignUp` and `SignIn`, comparing credentials against the `Users` table.
5. **Register the service** – In `Program.cs`, reference `AdminDashboard.Services` and call `builder.Services.AddScoped<IUserAuthService, DemoUserAuthService>();` after `AddDbContext`.
	Demo account cards only auto-fill the inputs—you must actually create those users (either via the `/signup` page or a quick seed) or you'll get “Invalid email or password.”

> 💡 **Copy/paste tip:** the tutorial components render code blocks inside Razor, so the source file in GitHub shows `@@` to escape `@`. If you copy directly from the repo, run a quick find/replace (`@@` → `@`) after pasting.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the `/signin` form and login flow.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step4`

## What Students Learn

1. How to build a login form with email and password.
2. How to verify credentials against data stored in the database.
3. How to set an authenticated user identity (cookie or simple session model for the tutorial).
4. How to show friendly error messages when login fails.

## Key Concepts

- Basic authentication patterns in Blazor Server
- Verifying credentials using EF Core queries
- Representing the current user in the app (claims/identity or custom model)
- Handling login errors and feedback

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step4` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `4`.
- Reuses the `User` entity and `AppDbContext` from Step 2 and accounts created in Step 3.

## Prerequisites

- Step 0: prerequisites & VS Code setup.
- Step 1: clean project + Tailwind.
- Step 2: auth model and DbContext.
- Step 3: working `/signup` page that creates users.

## Next Steps

- Proceed to Step 5 to build the `/profile` page for viewing and editing user information.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on the `/signin` UI, credential check, and setting the current user.

## Common Issues & Solutions

- **`@@bind` / `@@onclick` showing up in your pasted file**: Replace every `@@` with `@`. The double symbol only exists so the tutorial page can render Razor syntax.
- **Demo cards still fail**: Create matching records (e.g., `admin@demo.com`) by signing up those users first, or update the demo card emails to accounts you actually have.
- **`IUserAuthService.SignIn` missing**: Update the interface/service files from Step 3 to include a `SignIn` method and recompile.
- **Login always failing**: Double-check password comparison logic and queries.
- **User identity not persisting**: Ensure the auth mechanism (cookie/session) is correctly configured.
- **No clear error message**: Add a dedicated area on the page to show errors when login fails.

## Related Resources

- ASP.NET Core authentication concepts
- EF Core querying with LINQ
