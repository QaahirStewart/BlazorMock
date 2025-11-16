# SaaS Dashboard Learning Track Reference

This document summarizes the SaaS Dashboard learning track and how each step maps to routes and components in the app.

## Overview

- **Guide:** `/admin-guide` (SaaS Dashboard Guide)
- **Phases page:** `/admin-guide/phase/{id}` (see phase IDs below)
- **Production dashboard:** `/admin/dashboard`
- **Tutorial steps (admin project key):** steps **1–10**

The track follows the detailed design in `docs/admin-dashboard-plan.md`.

## Phases & Steps

### Phase 1 – Layout & Tiles (Steps 1–3)

- **Phase ID:** `admin-phase-1`
- **Phase route:** `/admin-guide/phase/admin-phase-1`
- **Goal:** Establish the dashboard layout, navbar entry, and reusable tiles/badges.

**Step 1 – Admin layout & navbar entry**

- **Step number:** `1`
- **Tutorial route:** `/admin-examples/step1`
- **Folder:** `Components/Pages/Examples/Admin/Step1/`
- **Demo component:** `AdminLayoutExample`
- **What it builds:**
  - Basic SaaS dashboard shell (header + content area).
  - Navbar entry that links to `/admin/dashboard`.

**Step 2 – Reusable dashboard stat card**

- **Step number:** `2`
- **Tutorial route:** `/admin-examples/step2`
- **Folder:** `Components/Pages/Examples/Admin/Step2/`
- **Demo component:** `AdminStatCardExample`
- **Reusable piece:** `AdminStatCard`
- **What it builds:**
  - Reusable stat card (title, value, helper text, status).
  - Used later across the dashboard and panels.

**Step 3 – Status badges & dashboard shell**

- **Step number:** `3`
- **Tutorial route:** `/admin-examples/step3`
- **Folder:** `Components/Pages/Examples/Admin/Step3/`
- **Demo component:** `AdminDashboardShellExample`
- **Reusable piece:** `AdminStatusBadge`
- **What it builds:**
  - Status badge component (e.g., Active, Trial, Overdue).
  - Grid-based dashboard shell for panels.

### Phase 2 – Roles & Locking (Steps 4–7)

- **Phase ID:** `admin-phase-2`
- **Phase route:** `/admin-guide/phase/admin-phase-2`
- **Goal:** Introduce roles, role toggle UI, and locked UI patterns.

**Step 4 – Auth service & admin roles**

- **Step number:** `4`
- **Tutorial route (planned):** `/admin-examples/step4`
- **Folder:** `Components/Pages/Examples/Admin/Step4/`
- **Demo component:** `AdminAuthServiceExample`
- **What it builds:**
  - In-memory `IAuthService` and `AuthUser` model.
  - Roles: `Regular` vs `SuperAdmin`.

**Step 5 – Role toggle & profile chip**

- **Step number:** `5`
- **Tutorial route (planned):** `/admin-examples/step5`
- **Folder:** `Components/Pages/Examples/Admin/Step5/`
- **Demo component:** `AdminRoleToggleExample`
- **What it builds:**
  - UI to switch roles (Regular/SuperAdmin).
  - Profile chip component used in the dashboard header.

**Step 6 – Locked tiles & disabled actions**

- **Step number:** `6`
- **Tutorial route (planned):** `/admin-examples/step6`
- **Folder:** `Components/Pages/Examples/Admin/Step6/`
- **Demo component:** `AdminLockPatternsExample`
- **What it builds:**
  - Lock overlay style for cards (icon + blur + explanation).
  - Disabled button patterns for SuperAdmin-only actions.

**Step 7 – Access explanation banner**

- **Step number:** `7`
- **Tutorial route (planned):** `/admin-examples/step7`
- **Folder:** `Components/Pages/Examples/Admin/Step7/`
- **Demo component:** `AdminAccessInfoExample`
- **What it builds:**
  - Reusable banner explaining why something is locked and how to unlock it ("Switch to SuperAdmin").

### Phase 3 – Panels & Final Dashboard (Steps 8–10)

- **Phase ID:** `admin-phase-3`
- **Phase route:** `/admin-guide/phase/admin-phase-3`
- **Goal:** Build the main content panels and wire them into `/admin/dashboard`.

**Step 8 – Subscription panel**

- **Step number:** `8`
- **Tutorial route (planned):** `/admin-examples/step8`
- **Folder:** `Components/Pages/Examples/Admin/Step8/`
- **Demo component:** `SubscriptionPanelExample`
- **Reusable piece:** `SubscriptionPanel`
- **What it builds:**
  - Subscription details (current plan, status, change-plan CTA).
  - Uses stat cards and badges from earlier steps.

**Step 9 – Billing summary panel**

- **Step number:** `9`
- **Tutorial route (planned):** `/admin-examples/step9`
- **Folder:** `Components/Pages/Examples/Admin/Step9/`
- **Demo component:** `BillingSummaryPanelExample`
- **Reusable piece:** `BillingSummaryPanel`
- **What it builds:**
  - Billing summary (next invoice, last payment, total spend, recent events).
  - Card-based layout suitable for mobile.

**Step 10 – Profile settings panel**

- **Step number:** `10`
- **Tutorial route (planned):** `/admin-examples/step10`
- **Folder:** `Components/Pages/Examples/Admin/Step10/`
- **Demo component:** `ProfileSettingsPanelExample`
- **Reusable piece:** `ProfileSettingsPanel`
- **What it builds:**
  - Profile form (name, email, notification toggles) with save/cancel patterns.
  - Ties into the same in-memory user state used by the profile chip.

## Production Dashboard Composition

- **Component:** `Components/Pages/AdminDashboard.razor`
- **Route:** `/admin/dashboard`
- **Composed from:**
  - Layout + header patterns from **Step 1**.
  - Stat tiles from **Step 2**.
  - Status badges and shell grid from **Step 3**.
  - Role/locking patterns from **Steps 4–7**.
  - Panels from **Steps 8–10**.

## Cross-References

- Detailed design and requirements: `docs/admin-dashboard-plan.md`.
- Tutorial step conventions: `TUTORIAL_TEMPLATE.md`.
- Progress metadata: `Services/LearningProgressService.cs` (admin steps 1–10).
- Phase definitions: `Services/LearningGuideService.cs` (admin-phase-1/2/3).
