# Admin Dashboard Step 11 — Desktop Navigation

## Overview

Step 11 mirrors the real desktop header that already lives in `Components/Layout/MainLayout.razor`. Students document the
same markup, explain how `NavLink` is styled, and show how the header reacts to auth changes so the guide stays aligned
with the shipping experience.

## Files in This Folder

- `Example.razor` — Tutorial page that embeds the production header, shows HTML-encoded snippets, and links back to the
  canonical layout file.
- `Example.razor.cs` — Code-behind for progress tracking, code-block JS, and the live demo helpers (iframe refresh + demo
  launch buttons).

## Routes

- Tutorial page: `/admin-dashboard-examples/step11`

## What Students Learn

1. How the centered desktop rail in `MainLayout.razor` is built with `NavLink` + custom `ActiveClass` styling.
2. How the right-hand column swaps between Sign In/Sign Up CTAs and the profile dropdown wired to `IUserAuthService`.
3. Why the layout subscribes to `Auth.OnChange` so the nav responds instantly to sign in/out.
4. How to test each persona (Free, Paid, Admin) using the quick-launch buttons and the embedded iframe.

## Key Concepts

- Layout-level navigation patterns
- `NavLink` active styling vs. `Match="NavLinkMatch.All"`
- Auth-driven UI updates via `IUserAuthService`
- Doc/demo parity (snippets taken directly from the real file)

## Architecture

- Tutorial page inherits `ExampleBase` in the `BlazorMock.Components.Pages.Examples.AdminDashboard.Step11` namespace.
- `ExampleBase` tracks completion for `admin-dashboard` step `11`, loads the `codeblocks.js` helper, and exposes
  `NavFrameSrc` + `Launch*Demo` helpers for the live preview.
- Content references the actual layout at `Components/Layout/MainLayout.razor` so the navigation story stays canonical.
- `MainLayout.razor` inspects the `?embed=nav` query string and hides the usual header/body wrappers so the Step 11
  iframe can display only the navigation chrome.

## Prerequisites

- Steps 0–10 completed (route protection from Step 10 ensures the nav links stay accurate).

## Next Steps

- Step 12 takes the same layout and focuses on the mobile drawer/hamburger experience.

## Code Structure

- `Example.razor` — Layout-focused tutorial with HTML-encoded snippets, notes, how-to checklist, and the iframe + quick
  launch demo block.
- `Example.razor.cs` — Progress tracking, JS interop bootstrapping, iframe refresh logic, and helper methods that open
  `/signin?demo=...` in a new tab.

## Common Issues & Solutions

- **Active state missing** — Ensure the `href` matches the canonical route and only `Guide` uses
  `Match="NavLinkMatch.All"`.
- **Nav never re-renders** — Subscribe/unsubscribe to `Auth.OnChange` inside the layout (as shown in the snippet) so the
  UI reacts to sign in/out.
- **Admin link hidden for admins** — Confirm the seeded account still has `Role == "Admin"` and the dropdown includes the
  conditional link.

## Related Resources

- `MainLayout.razor` (canonical header implementation)
- Tips: `#blazor-navlink`, `#layout-state`, `#auth-service`
