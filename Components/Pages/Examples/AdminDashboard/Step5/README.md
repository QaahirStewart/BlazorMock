# Admin Dashboard Step 5 — Profile Page (/profile)

## Overview

This step adds a `/profile` page where the currently signed-in user can view and update their information.

## Quick Start (Copy/Paste Friendly)

1. **Match your namespace** – Replace `AdminDashboard.*` with your project's root namespace (or set `&lt;RootNamespace&gt;YourApp&lt;/RootNamespace&gt;` in the `.csproj`).
2. **Bring Steps 2–4 along** – You need the `User` model, DbContext, EF Core packages, and working `/signup` + `/signin` pages before tackling `/profile`.
3. **Update `Models/User.cs`** – Add the `FullName`, `ProfilePictureUrl`, `CreatedAt`, and `LastLoginAt` properties (snippet in this step) so profile data exists. Run:
   ```pwsh
   dotnet ef migrations add AddProfileFields
   dotnet ef database update
   ```
4. **Extend `IUserAuthService`** – Ensure the interface exposes `User? CurrentUser`, plus `SignOut`, `UpdateProfile`, and `UpgradeToRole`. Replace the file with the provided snippet if needed.
5. **Replace the service implementation** – Use the updated `DemoUserAuthService` shown here so the new methods and `CurrentUser` state compile in a clean Blazor app.
6. **DI registration unchanged** – `Program.cs` still needs `builder.Services.AddScoped<IUserAuthService, DemoUserAuthService>();` after `AddDbContext`.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the `/profile` UI and logic.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step5`

## What Students Learn

1. How to load the current user’s data based on their identity.
2. How to display user information (email, display name, role) in a form.
3. How to allow users to edit certain fields (e.g., display name) and save changes.
4. How to give clear feedback when profile updates succeed or fail.

## Key Concepts

- Getting the current user from the auth context
- Loading and updating entities with EF Core
- Editable versus read-only profile fields
- UX patterns for saving and confirming changes

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step5` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `5`.
- Relies on the auth model and login from Steps 2–4.

## Prerequisites

- Steps 0–4 completed, especially `/signin` so there is a current user.

## Next Steps

- Proceed to Step 6 to introduce an analytics event model for tracking user activity.

## Code Structure

- `ExampleBase` handles completion tracking and JS enhancements.
- The tutorial page focuses on data loading, editing, and saving for the profile.

## Common Issues & Solutions

- **`CurrentUser`/`UpdateProfile` missing**: Update `IUserAuthService` and `DemoUserAuthService` with the versions in this step; older copies from Step 3 lacked these members.
- **Profile not loading**: Confirm there is a signed-in user and that their ID is available.
- **Changes not saving**: Check EF Core tracking and ensure `SaveChangesAsync` is called.
- **Unauthorized access**: Add a simple guard to redirect users who are not signed in.

## Related Resources

- EF Core tracking and updates
- Blazor forms and data binding
