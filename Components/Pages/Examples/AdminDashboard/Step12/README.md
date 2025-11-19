# Admin Dashboard Step 12 — Mobile Navigation & Final Polish

## Overview

Step 12 keeps the tutorial’s documentation in lockstep with the real `MainLayout.razor`. It highlights the production
hamburger button, the animated mobile drawer, and the code-behind helpers that keep auth + navigation state in sync on
phones.

## Files in This Folder

- `Example.razor` — Tutorial content with the canonical layout snippets, live nav embed, and QA checklist.
- `Example.razor.cs` — Progress tracking + iframe refresh helper for the embedded nav preview.

## Routes

- Tutorial page: `/admin-dashboard-examples/step12`

## What Students Learn

1. How the existing layout swaps between desktop links and the mobile drawer (`md:hidden` / `hidden md:block`).
2. How `ToggleMobileMenu`, `CloseMobileMenu`, and `HandleSignOut` keep the drawer responsive to auth changes.
3. How to QA Free/Paid/Admin personas on narrow screens using the `/?embed=nav` live preview.
4. How to perform the final UX polish lap so the tutorial, screenshots, and production layout never drift.

## Key Concepts

- Single-source layouts & snippets
- Responsive breakpoints and animated drawers
- Auth-driven UI updates via `InvokeAsync(StateHasChanged)`
- Device toolbar testing and manual persona verification

## Architecture

- `ExampleBase` (Step 12) inherits from `ComponentBase`, injects `ILearningProgressService`, and now exposes a
  timestamped `NavFrameSrc` plus a `RefreshNavFrame()` handler for the iframe.
- The Razor page embeds real chunks from `Components/Layout/MainLayout.razor` and documents the same code paths used in
  production.

## Prerequisites

- Steps 9–11 completed (guards + desktop nav) so the mobile drawer has the correct data/state to reflect.

## Next Steps

- After finishing Step 12, return to `/admin-dashboard-guide` to review overall progress or branch off into custom
  features using the same layout.

## Code Structure

- Hero + “What you’ll learn” section summarising the responsive goals.
- Canonical snippet blocks for the header, drawer, and `@@code` section from `MainLayout.razor`.
- QA checklist + live `/?embed=nav` iframe wrapped in a phone shell.
- Completion CTA consistent with earlier steps.

## Common Issues & Solutions

- **Drawer never closes** → ensure each `NavLink` calls `CloseMobileMenu()` via `@onclick` and that sign-out resets the
  flag.
- **Auth state stale between tabs** → double-check `Auth.OnChange += OnAuthStateChanged` and the dispatcher-safe
  `InvokeAsync(StateHasChanged)` callback.
- **Active link styling wrong** → copy the `ActiveClass` values from `MainLayout.razor` so colors match across docs and
  production.

## Related Resources

- `/tips#auth-service` — reusing the shared auth service.
- `/tips#layout-state` — keeping layout state centralised.
- `/tips#tailwind` — responsive Tailwind utility references.
