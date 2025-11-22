# Admin Dashboard Step 12 — Mobile Navigation & Final Polish

## Overview

Step 12 finishes the dedicated `AdminNav` component you authored in Step 11 by adding the animated mobile drawer,
closing handlers, and final QA script. Everything happens inside the clean AdminDashboard app—no edits are made to this
repo’s `MainLayout.razor`.

## Files in This Folder

- `Example.razor` — Tutorial content with the canonical mobile drawer snippets, live nav embed, and QA checklist.
- `Example.razor.cs` — Progress tracking + iframe refresh helper for the embedded nav preview.

## Routes

- Tutorial page: `/admin-dashboard-examples/step12`

## What Students Learn

1. How to append the `md:hidden` drawer to `Components/Layout/AdminNav.razor` in the clean AdminDashboard project.
2. How `ToggleMobileMenu`, `CloseMobileMenu`, and `HandleSignOut` keep the drawer responsive to auth changes.
3. How to QA Free/Paid/Admin personas on narrow screens using the shared `/?embed=nav` preview you wired up in Step 11.
4. How to run a final polish lap so the tutorial, screenshots, and the fresh project stay aligned.

## Key Concepts

- Single-source layouts & snippets
- Responsive breakpoints and animated drawers
- Auth-driven UI updates via `InvokeAsync(StateHasChanged)`
- Device toolbar testing and manual persona verification

## Architecture

- `ExampleBase` (Step 12) inherits from `ComponentBase`, injects `ILearningProgressService`, and exposes the same
  `NavFrameSrc` + `RefreshNavFrame()` helpers you used in Step 11.
- Content references the student-authored `Components/Layout/AdminNav.razor` file inside the clean AdminDashboard app
  (not this repo’s layout) and documents the exact code paths learners should maintain.

## Prerequisites

- Steps 9–11 completed (guards + the desktop nav component) so the drawer has accurate routes and auth state.

## Next Steps

- After finishing Step 12, return to `/admin-dashboard-guide` to review progress or branch into custom features using
  the new nav component as your template.

## Code Structure

- Hero + “What you’ll learn” section summarising the responsive goals.
- Canonical snippet blocks for the `AdminNav.razor` drawer and its shared `AdminNav.razor.cs` code-behind.
- QA checklist + live `/?embed=nav` iframe wrapped in a phone shell.
- Completion CTA consistent with earlier steps.

## Common Issues & Solutions

- **Drawer never closes** → ensure each `NavLink` calls `CloseMobileMenu()` via `@onclick` and that sign-out resets the
  flag.
- **Auth state stale between tabs** → double-check `Auth.OnChange += OnAuthStateChanged` and the dispatcher-safe
  `InvokeAsync(StateHasChanged)` callback.
- **Build errors referencing `IsAuthenticated`/`OnChange`** → copy the updated `IUserAuthService`/`DemoUserAuthService`
  contract from Step 11 so the nav component compiles.
- **Active link styling wrong** → copy the `ActiveClass` values from your `AdminNav.razor` component so docs and the
  clean app stay pixel-perfect.

## Related Resources

- `/tips#auth-service` — reusing the shared auth service.
- `/tips#layout-state` — keeping layout state centralised.
- `/tips#tailwind` — responsive Tailwind utility references.
