# Admin Dashboard Step 11 — Desktop Navigation

## Overview

Step 11 no longer copies the nav that ships in this repo. Instead, you author a dedicated header for the fresh
AdminDashboard app you built in Steps 1–10. The lesson walks through creating `Components/Layout/AdminNav.razor`, wiring
it into your clean solution’s layout, and aligning every link with the routes you’ve implemented so far (`/`,
`/signin`, `/signup`, `/profile`, `/analytics`, `/admin/dashboard`).

## Files in This Folder

- `Example.razor` — Documentation page that details the new nav component, highlights the desktop UX, and shows
  HTML-encoded snippets learners can paste into their clean project.
- `Example.razor.cs` — Code-behind for progress tracking, code-block JS, and the live preview helpers (unchanged from
  earlier steps).

## Routes

- Tutorial page: `/admin-dashboard-examples/step11`

## What Students Learn

1. How to create a reusable `AdminNav` component with a centered desktop rail for the AdminDashboard routes.
2. How to swap the right-hand column between CTA buttons and the persona-aware profile dropdown using
   `IUserAuthService`.
3. How to keep docs and the fresh project in sync by sourcing snippets from the same component file.
4. How to update `IUserAuthService`/`DemoUserAuthService` so `IsAuthenticated` and `OnChange` exist for the nav.
5. How to light up deterministic auth previews (via `?embed=nav`) without touching the legacy BlazorMock
   `MainLayout.razor`.

## Key Concepts

- Component-scoped navigation (no longer tied to BlazorMock’s layout)
- `NavLink` active styling + route parity across desktop and mobile sections
- Auth-driven UI updates through `IUserAuthService.OnChange`
- Service contract updates (bool `IsAuthenticated`, `Action? OnChange`) so components compile cleanly
- Documentation-first workflow: author once in `AdminNav.razor`, mirror it in the tutorial

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step11` namespace.
- Students create `Components/Layout/AdminNav.razor` plus the matching `AdminNav.razor.cs` partial class inside their
  clean solution.
- Their `MainLayout.razor` (in the fresh app) simply renders `<AdminNav />` and keeps the optional `?embed=nav` toggle so
  the doc iframe can isolate the header.
- The guidance does **not** modify this repo’s `Components/Layout/MainLayout.razor`; everything is scoped to the clean
  AdminDashboard project.

## Prerequisites

- Steps 0–10 completed (guards from Step 10 ensure `/profile`, `/analytics`, and `/admin/dashboard` respect auth state).

## Next Steps

- Step 12 extends the same component with the mobile drawer/hamburger UX and runs the final nav polish lap.

## Code Structure

- `Example.razor` — Shows the `AdminNav.razor` desktop markup, embeds the live preview, and outlines the desk-only test
  checklist.
- `Example.razor.cs` — Handles completion state, JS enhancements, and the iframe refresh button.

## Common Issues & Solutions

- **Missing members on `IUserAuthService`** — Add `bool IsAuthenticated { get; }` and `event Action? OnChange` to the
  interface, then raise the event in your demo service after sign-in/out, profile edits, and admin actions.
- **Links don’t match your app** — Double-check the `NavLink` list uses `/`, `/profile`, `/analytics`, and
  `/admin/dashboard` (not the BlazorMock demo routes).
- **Nav never re-renders after sign-in** — Ensure the component subscribes/unsubscribes to `Auth.OnChange` and calls
  `InvokeAsync(StateHasChanged)` just like earlier auth-aware components.
- **Admin link missing for admins** — Confirm your seeded admin user still has `Role == "Admin"` inside your fresh
  database seed.

## Related Resources

- Step 10 (guards) — proves the routes you link to already enforce auth.
- Step 12 — finishes the mobile drawer for the same component.
- Tips: `#auth-service`, `#layout-state`, `#blazor-navlink`
