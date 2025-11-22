# Blazor Learning Guides – Layout & Style Reference

Trucking is the **source of truth** for layout and structure. Other projects (Pokémon, Admin) should follow the same structure but can use their own color palettes.

Use this as a checklist when building or updating any guide.

---

## 1. Page Shell

- **Route**: project-specific (`/trucking-guide`, `/pokemon-guide`, `/admin-guide`).
- **Wrapper**: `div.min-h-screen.py-8` → `div.w-full.mx-auto.px-4.max-w-5xl`.
- **Typography**: Tailwind-style utility classes with `text-gray-*` and `font-bold` for key headings.

```razor
<div class="min-h-screen py-8">
    <div class="w-full mx-auto px-4 max-w-5xl">
        <!-- content -->
    </div>
</div>
```

---

## 2. Guide Header

**Purpose**: Big, friendly intro at the top of each project guide.

- **Structure**:
  - Centered block: `div.text-center.mb-12`.
  - Main title: `h1.text-3xl sm:text-4xl md:text-5xl.font-bold.text-gray-900.mb-4.break-words`.
  - Subtitle: `p.text-base sm:text-lg md:text-xl.text-gray-600.mb-4.leading-relaxed.break-words`.
- **Content rules**:
  - Start with an emoji that matches the project.
  - Title calls out the project name.
  - Subtitle explains the app and learning focus in 1–2 sentences.

**Example – Trucking**

```razor
<div class="text-center mb-12">
    <h1 class="text-3xl sm:text-4xl md:text-5xl font-bold text-gray-900 mb-4 break-words">
        🚚 Trucking Learning Guide
    </h1>
    <p class="text-base sm:text-lg md:text-xl text-gray-600 mb-4 leading-relaxed break-words">
        Step-by-step guide to building a Trucking Schedule App
    </p>
</div>
```

**Example – Pokémon**

```razor
<div class="text-center mb-12">
    <h1 class="text-3xl sm:text-4xl md:text-5xl font-bold text-gray-900 mb-4 break-words">
        🟡 Pokemon Collector Guide
    </h1>
    <p class="text-base sm:text-lg md:text-xl text-gray-600 mb-4 leading-relaxed break-words">
        Learn data fetching, search, and client-side state by building a Pokemon Collector.
    </p>
</div>
```

> Other projects: copy the structure, change the emoji + text.

---

## 3. "Learning Phases" Banner

**Purpose**: A soft gradient banner that introduces the phase cards.

- **Placement**: Directly below the header.
- **Structure**:
  - Container: `div.bg-gradient-to-r.from-gray-100.to-blue-100.rounded-2xl.p-5.mt-12`.
  - Title: `h2.text-2xl sm:text-3xl.font-bold.text-gray-900.mb-2.break-words`.
  - Copy: `p.text-sm sm:text-base.text-gray-700.break-words`.
- **Content**: Always use the same text for title and description unless there’s a strong reason to change.

```razor
<div class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 mt-12">
    <h2 class="text-2xl sm:text-3xl font-bold text-gray-900 mb-2 break-words">🔰 Learning Phases</h2>
    <p class="text-sm sm:text-base text-gray-700 break-words">
        Pick a phase to focus. Each card links to steps with examples.
    </p>
</div>
```

> Palette note: keep the neutral gray→blue gradient for consistency across projects.

---

## 4. Phase Cards Grid (Guide Page)

**Purpose**: Show each phase as a card with progress and a colored pill.

- **Placement**: Below the Learning Phases banner.
- **Grid**: `div.grid.sm:grid-cols-2.lg:grid-cols-3.gap-6.my-10.max-w-6xl.mx-auto`.
- **Card anchor**:
  - Base: `rounded-2xl p-0 bg-white border-2 border-gray-200 hover:shadow-lg transition-all text-left block`.
  - Plus a per-phase hover class via a helper.
- **Card body** (`div.p-8`):
  - Top row: flex with text + arrow.
  - Pill: `inline-flex.items-center.px-2.py-1.rounded-full.text-xs.font-medium.mb-2` + helper for colors.
  - Title: `h3.text-2xl.font-bold.text-gray-900.mb-1.break-words`.
  - Description: `p.text-sm.sm:text-base.text-gray-700.leading-relaxed.break-words`.
  - Progress line: `div.text-sm.text-gray-600` → `Progress: {completedInPhase}/{totalInPhase}`.

**Canonical structure**

```razor
<div class="grid sm:grid-cols-2 lg:grid-cols-3 gap-6 my-10 max-w-6xl mx-auto">
    @foreach (var ph in _phases)
    {
        var count = _steps.Count(s => s.IsComplete && ph.StepNumbers.Contains(s.StepNumber));
        <a href="@($"/PROJECT-ROUTE/phase/{ph.Id}")"
           class="@($"rounded-2xl p-0 bg-white border-2 border-gray-200 hover:shadow-lg transition-all text-left block {PhaseHoverBorder(ph.Id)}")">
            <div class="p-8">
                <div class="flex items-start justify-between gap-4 mb-3">
                    <div class="min-w-0">
                        <div class="@($"inline-flex items-center px-2 py-1 rounded-full text-xs font-medium mb-2 {PhasePillColors(ph.Id)}")">
                            @ph.Subtitle
                        </div>
                        <h3 class="text-2xl font-bold text-gray-900 mb-1 break-words">@ph.Title</h3>
                        <p class="text-sm sm:text-base text-gray-700 leading-relaxed break-words">@ph.Description</p>
                    </div>
                    <div class="text-gray-400 self-start">→</div>
                </div>
                <div class="text-sm text-gray-600">Progress: @count/@ph.StepNumbers.Length</div>
            </div>
        </a>
    }
</div>
```

### Phase color helpers

Each project gets its own palette but uses the same helper names and structure.

**Trucking (reference)**

```csharp
private static string PhaseHoverBorder(string id) => id switch
{
    "phase-1" => "hover:border-purple-400",
    "phase-2" => "hover:border-blue-400",
    "phase-3" => "hover:border-emerald-400",
    _ => "hover:border-gray-300"
};

private static string PhasePillColors(string id) => id switch
{
    "phase-1" => "bg-purple-50 text-purple-700 border border-purple-200",
    "phase-2" => "bg-blue-50 text-blue-700 border border-blue-200",
    "phase-3" => "bg-emerald-50 text-emerald-700 border border-emerald-200",
    _ => "bg-gray-50 text-gray-700 border border-gray-200"
};
```

**Pokémon (own palette)**

```csharp
private static string PhaseHoverBorder(string id) => id switch
{
    "pokemon-phase-1" => "hover:border-yellow-400",
    "pokemon-phase-2" => "hover:border-blue-400",
    "pokemon-phase-3" => "hover:border-emerald-400",
    _ => "hover:border-gray-300"
};

private static string PhasePillColors(string id) => id switch
{
    "pokemon-phase-1" => "bg-yellow-50 text-yellow-700 border border-yellow-200",
    "pokemon-phase-2" => "bg-blue-50 text-blue-700 border border-blue-200",
    "pokemon-phase-3" => "bg-emerald-50 text-emerald-700 border border-emerald-200",
    _ => "bg-gray-50 text-gray-700 border border-gray-200"
};
```

> Admin and future projects: copy this pattern; only change the color tokens and phase IDs.

---

## 5. Phase Detail Page Layout

**Reference**: `TruckingPhase.razor` (trucking) and `PokemonPhase.razor`.

### Back link

- Top-left link back to the project’s guide.

```razor
<a href="/PROJECT-GUIDE" class="inline-flex items-center gap-2 text-blue-600 hover:text-blue-700 mb-4 text-sm sm:text-base">
    <span>←</span> Back to PROJECT phases
</a>
```

### Phase header

- **Container**: `div.rounded-2xl.p-5.sm:p-6.mb-8` with a gradient background.
- **Gradient**:
  - Trucking uses `PhaseHeaderGradient(phase.Id)` to pick per-phase colors.
  - Pokémon uses a fixed gradient: `bg-gradient-to-r from-yellow-50 via-indigo-50 to-blue-100`.
- **Content**:
  - Small pill at top.
  - Big phase title.
  - Short description.
  - Progress line: `Progress: X/Y`.

**Canonical Pokémon-style header**

```razor
<div class="rounded-2xl p-5 sm:p-6 mb-8 bg-gradient-to-r from-yellow-50 via-indigo-50 to-blue-100">
    <div class="mb-3">
        <span class="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-purple-50 text-purple-700 border border-purple-200">
            API Focus
        </span>
    </div>
    <h1 class="text-2xl sm:text-3xl md:text-4xl font-bold text-gray-900 mb-2 break-words">@phase!.Title</h1>
    <p class="text-sm sm:text-base text-gray-700 mb-3 break-words">@phase.Description</p>
    <p class="text-sm sm:text-base text-gray-800 font-semibold">
        Progress: @stepsInPhase.Count(s => s.IsComplete)/@stepsInPhase.Count
    </p>
</div>
```

> For trucking, use per-phase gradient + per-phase pill colors. For other projects, pick a palette but keep the same structure.

### Steps list

- **Layout**: `div.grid.sm:grid-cols-2.gap-4`.
- Each step card:
  - White rounded card with border.
  - Left badge: number or checkmark.
  - Title and optional completed date/label.
  - Right arrow icon.
  - Status + toggle button below.

This mirrors the trucking phase card feel while allowing per-project text.

---

## 6. Learning Resources & Progress Block (Guide Page)

**Purpose**: A dark panel with quick links to demos/progress/tips.

- **Outer wrapper**: `div.bg-black/3.rounded-2xl`.
- **Inner panel**: `div.bg-black/90.rounded-2xl.p-5.sm:p-6.text-white`.
- **Title**: centered line `"📚 Learning Resources"`.
- **Grid of links**: 2–3 cards for demos, progress page, tips, or hub.
  - Card: `bg-white/10.hover:bg-white/20.rounded-xl.p-4.transition-colors`.
  - Icon: `span.text-2xl.shrink-0`.
  - Title: `div.text-sm.sm:text-base.font-semibold`.
  - Description: `div.text-xs.sm:text-sm.text-gray-300`.
- **Footer progress line**:

```razor
<div class="text-center text-xs sm:text-sm text-gray-300 break-words" hidden="@(_steps is null)">
    <strong>Progress:</strong> @_completedCount / @(_steps?.Count ?? 0) steps complete
</div>
```

**Example links (Pokémon)**

- `/progress` – project progress.
- `/tips` – tips system.
- `/guide` – back to project hub.

Projects can swap icons or link labels, but keep the structure.

---

## 7. Progress Page Conventions (`Progress.razor`)

Each project’s section on the progress page should:

- Use a card with:
  - Project icon and title.
  - "X / Y steps complete" line.
  - Horizontal progress bar (% complete).
  - List of steps with toggles.
- Keep layout consistent, but allow small palette tweaks per project.

---

## 8. Project Keys & Phases

- **Project keys**: `"trucking"`, `"pokemon"`, `"admin"`.
- `LearningGuideService` defines a list of `Phase` records with:
  - `Id` (e.g., `"phase-1"`, `"pokemon-phase-1"`, `"admin-phase-1"`).
  - `Title`, `Subtitle`, `Description`.
  - `StepNumbers`.
  - `ProjectKey`.
- `GetPhases(projectKey)` and `GetPhaseById(projectKey, id)` provide phase data to guide pages.

Guides should always use the project-aware APIs:

```csharp
_phases = GuideService.GetPhases("pokemon");

phase = GuideService.GetPhaseById("pokemon", Id);
```

---

## 9. When Adding a New Project

1. **Define phases** in `LearningGuideService` with a new `ProjectKey` and unique `Id`s.
2. **Create a guide page** `Components/Pages/NewProjectGuide.razor`:
   - Copy structure from `TruckingGuide.razor` or `PokemonGuide.razor`.
   - Update header emoji/text.
   - Implement `PhaseHoverBorder` and `PhasePillColors` with a project-specific palette.
3. **Create a phase page** `Components/Pages/NewProjectPhase.razor`:
   - Copy from `PokemonPhase.razor`.
   - Adjust gradient and pill text to fit the project.
4. **Update the hub** (`GuideHub.razor`) to add a card for the new project.
5. **Update `Progress.razor`** to add a project card with progress bar and steps.

If you follow this checklist, every new project will feel like part of the same learning family while keeping its own color story and theme.
