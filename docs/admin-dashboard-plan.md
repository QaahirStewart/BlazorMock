# Admin SaaS Dashboard Learning Track Plan

This document defines the new Admin SaaS dashboard learning path. It mirrors the other projects (Trucking, Pokémon) with a 3–4–3 phase structure. Each step is an `Examples/Admin/Step{N}` tutorial that builds a real piece of the final production dashboard at `/admin/dashboard`.

## High-Level Goals

- Build a **SaaS-style Admin dashboard** focusing on subscription/billing and profile management.
- Use **Regular** vs **Super Admin** roles to demonstrate role-based UI and locked/unlocked behavior.
- Keep the **final dashboard** as a standalone production route (`/admin/dashboard`), **separate** from tutorial routes.
- Make each tutorial step a **demo piece** that plugs directly into the final dashboard.
- Follow `TUTORIAL_TEMPLATE.md` for all tutorial steps (structure, gradients, tips, code block rules).

## Final Production Dashboard

- Component: `Components/Pages/AdminDashboard.razor` + `AdminDashboard.razor.cs`.
- Route: `@page "/admin/dashboard"` (no `/admin-examples/*`).
- Layout:
  - Reuses the main app navbar (no sidebar) with an **Admin** link.
  - Shows a profile/role chip in the top-right (Regular vs Super Admin).
- Panels:
  - **SubscriptionPanel** – current plan, status, upgrade/downgrade CTAs.
  - **BillingSummaryPanel** – next invoice, total spend, recent billing events.
  - **ProfileSettingsPanel** – name/email/notification toggles.
- Role behavior:
  - Regular admin: can view everything but some actions and settings are **locked**.
  - Super admin: can perform all actions (unlocking dangerous operations and configuration).
- Locked UI patterns:
  - Disabled buttons, lock icons, hover text or banner explaining what is required.
  - Uses reusable "access explanation" components from Phase 2.

## Phases and Steps (3–4–3)

### Phase 1 – Layout & Tiles (Steps 1–3)

Goal: establish the admin layout, navbar entry, and reusable dashboard cards/badges.

#### Step 1: Admin Layout & Navbar Entry

- Tutorial route: `/admin-examples/step1`.
- Folder: `Components/Pages/Examples/Admin/Step1/`.
- Demo concept: `AdminLayoutExample`.
- Files:
  - `Example.razor` / `Example.razor.cs` (tutorial page using `ExampleBase`).
  - `AdminLayoutExample.razor` / `AdminLayoutExample.razor.cs`.
  - `README.md` explaining layout goals and routes.
- What this builds:
  - Basic admin page shell with heading, content area, and background.
  - Navbar updated to include an **Admin** link pointing to `/admin/dashboard` (even if empty initially).
- How it connects to `/admin/dashboard`:
  - The layout pattern and base classes are reused in `AdminDashboard.razor`.

#### Step 2: Reusable Dashboard Card Component

- Tutorial route: `/admin-examples/step2`.
- Folder: `Components/Pages/Examples/Admin/Step2/`.
- Demo concept: `AdminStatCardExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminStatCardExample.razor` / `AdminStatCardExample.razor.cs`.
  - `README.md`.
- What this builds:
  - A reusable **stat card** (title, value, badge, description).
  - Used later for subscription, billing, and profile summary tiles.
- How it connects to `/admin/dashboard`:
  - `AdminDashboard.razor` composes multiple stat cards to show key metrics.

#### Step 3: Status Badges & Dashboard Shell

- Tutorial route: `/admin-examples/step3`.
- Folder: `Components/Pages/Examples/Admin/Step3/`.
- Demo concept: `AdminDashboardShellExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminDashboardShellExample.razor` / `AdminDashboardShellExample.razor.cs`.
  - `README.md`.
- What this builds:
  - Badge component(s) (e.g., "Active", "Trial", "Overdue").
  - A simple **shell layout** with a grid where panels will live.
- How it connects to `/admin/dashboard`:
  - The grid structure and badges are reused in the real dashboard to display plan/status.

### Phase 2 – Roles & Locking (Steps 4–7)

Goal: introduce a mock auth service, roles, and locked UI patterns.

#### Step 4: Auth Service & Admin Roles

- Tutorial route: `/admin-examples/step4`.
- Folder: `Components/Pages/Examples/Admin/Step4/`.
- Demo concept: `AdminAuthServiceExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminAuthServiceExample.razor` / `AdminAuthServiceExample.razor.cs`.
  - `README.md`.
- What this builds:
  - Simple in-memory `IAuthService` and `AuthUser` model.
  - Roles: `"Regular"` and `"SuperAdmin"`.
- How it connects to `/admin/dashboard`:
  - `AdminDashboard.razor` injects `IAuthService` to determine the current role and decide what to lock/unlock.

#### Step 5: Role Toggle & Profile Chip

- Tutorial route: `/admin-examples/step5`.
- Folder: `Components/Pages/Examples/Admin/Step5/`.
- Demo concept: `AdminRoleToggleExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminRoleToggleExample.razor` / `AdminRoleToggleExample.razor.cs`.
  - `README.md`.
- What this builds:
  - UI to toggle between Regular/Super admin (buttons or segmented control).
  - Small profile chip/avatar component showing current role.
- How it connects to `/admin/dashboard`:
  - The profile chip is embedded in the top-right of the dashboard layout.
  - `AdminDashboard.razor` uses the same toggle pattern to simulate signing in as different roles.

#### Step 6: Locked Tiles & Disabled Actions

- Tutorial route: `/admin-examples/step6`.
- Folder: `Components/Pages/Examples/Admin/Step6/`.
- Demo concept: `AdminLockPatternsExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminLockPatternsExample.razor` / `AdminLockPatternsExample.razor.cs`.
  - `README.md`.
- What this builds:
  - Reusable lock overlay style for cards (icon + blur + text).
  - Disabled buttons with tooltip-style explanation.
- How it connects to `/admin/dashboard`:
  - The lock overlay and disabled states are reused for billing configuration and other super-admin-only actions.

#### Step 7: Access Explanation Banner

- Tutorial route: `/admin-examples/step7`.
- Folder: `Components/Pages/Examples/Admin/Step7/`.
- Demo concept: `AdminAccessInfoExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `AdminAccessInfoExample.razor` / `AdminAccessInfoExample.razor.cs`.
  - `README.md`.
- What this builds:
  - A reusable banner component that explains why something is locked and **how to unlock it** (e.g., "Switch to Super admin role").
- How it connects to `/admin/dashboard`:
  - `AdminDashboard.razor` shows this banner above locked sections to reinforce role-based access.

### Phase 3 – Panels & Final Pieces (Steps 8–10)

Goal: build the main content panels and then wire them into the production dashboard.

#### Step 8: Subscription Panel

- Tutorial route: `/admin-examples/step8`.
- Folder: `Components/Pages/Examples/Admin/Step8/`.
- Demo concept: `SubscriptionPanelExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `SubscriptionPanelExample.razor` / `SubscriptionPanelExample.razor.cs`.
  - `README.md`.
- What this builds:
  - `SubscriptionPanel` UI with current plan, status, and simple change plan actions.
  - Uses stat cards and badges from earlier steps.
- How it connects to `/admin/dashboard`:
  - `AdminDashboard.razor` renders the real `SubscriptionPanel` in the left column.

#### Step 9: Billing Summary Panel

- Tutorial route: `/admin-examples/step9`.
- Folder: `Components/Pages/Examples/Admin/Step9/`.
- Demo concept: `BillingSummaryPanelExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `BillingSummaryPanelExample.razor` / `BillingSummaryPanelExample.razor.cs`.
  - `README.md`.
- What this builds:
  - Billing summary UI (next invoice, last payment, total spend, recent events list).
  - Uses card layout instead of tables for responsiveness.
- How it connects to `/admin/dashboard`:
  - The same panel is nested in the main dashboard grid under the subscription panel.

#### Step 10: Profile Settings Panel

- Tutorial route: `/admin-examples/step10`.
- Folder: `Components/Pages/Examples/Admin/Step10/`.
- Demo concept: `ProfileSettingsPanelExample`.
- Files:
  - `Example.razor` / `Example.razor.cs`.
  - `ProfileSettingsPanelExample.razor` / `ProfileSettingsPanelExample.razor.cs`.
  - `README.md`.
- What this builds:
  - Profile settings UI (name, email, notification toggles) and save/cancel patterns.
  - Demonstrates basic form binding + validation.
- How it connects to `/admin/dashboard`:
  - `AdminDashboard.razor` uses this panel on the right-hand side, wired to the same in-memory state as the profile chip.

## Routes and Naming Summary

- Tutorial pages: `/admin-examples/step{n}` (1–10).
- Demo components (optional): `/admin-examples/demo-{feature}` (e.g., `demo-layout`, `demo-role-toggle`).
- Guide: `/admin-guide`.
- Phases page: `/admin-guide/phase/{id}`.
- Production dashboard: `/admin/dashboard`.

## Tips & Related Topics

Each step must:

- Use the **"What You'll Learn"** gradient banner and Key Concepts footer from `TUTORIAL_TEMPLATE.md`.
- Link to real tips in `TipsService` for concepts such as:
  - "SaaS Admin Dashboard Layout" (layout + card grids).
  - "Role-Based Authorization" (Regular vs Super admin).
  - "Locked UI States" (disabled, overlays, badges).
  - "Subscription Billing Panels" (subscription UI patterns).
- Verify all anchors (`/tips#slug`) match the slug rules in `TUTORIAL_TEMPLATE.md`.

## Implementation Checklist

- [ ] Remove legacy Admin steps and demos that no longer match this design.
- [ ] Update `LearningGuideService` to use Admin steps 1–10 in a 3–4–3 phase map.
- [ ] Ensure `AdminGuide.razor`, `AdminPhase.razor`, `GuideHub.razor`, and `Progress.razor` copy matches the new SaaS Admin story.
- [ ] For each step:
  - [ ] Create the `Components/Pages/Examples/Admin/Step{N}/` folder.
  - [ ] Add `Example.razor`, `Example.razor.cs`, `{PieceName}Example.razor`, `{PieceName}Example.razor.cs`, and `README.md`.
  - [ ] Follow `TUTORIAL_TEMPLATE.md` for structure, sections, code blocks, and escaping `@@`.
  - [ ] Add any new tips to `TipsService` before linking.
  - [ ] Run `dotnet build -c Release` and verify `/tips` and `/admin-examples/step{n}`.
- [ ] Implement `AdminDashboard.razor` to compose all pieces into the final `/admin/dashboard` experience.
