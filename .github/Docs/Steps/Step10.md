# Step 10 — State Management

- Live example: /examples/step10
- Next step: [Step 11 — Assignment Logic & Business Rules](./Step11.md)
- Previous: [Step 09 — CRUD Operations & Related Data](./Step09.md)

What you'll do:

- Create scoped AppState service
- Share state across components
- Build a small demo with a DB-backed driver picker and a selection card

Summary:

- Centralize shared state in a scoped service.
- Inject and use state across multiple components.
- Avoid prop-drilling for common values.

Live Demo (in /examples/step10):

- Two components share a scoped `AppState`:
  - `DriverPicker.razor` loads drivers from the database (`IDbContextFactory<AppDbContext>`) and updates `AppState.SelectedDriver`.
  - `SelectedDriverCard.razor` subscribes to `AppState.OnChange` and displays the current selection with a Clear action.
- The example page includes collapsible "Show code" blocks with Copy buttons so you can reuse the exact component code.

Key Patterns:

- Scoped service per user/circuit for UI state: `Services/AppState.cs`
- Event notification via `Action OnChange` + `StateHasChanged()`
- Always unsubscribe from events in `Dispose()` to avoid memory leaks

## How to add in‑app code samples

- Use a white card, then a gray container to visually group the sample.
- Insert a raw `<pre data-code-title="...">` block; the site enhancer adds Show code + Copy automatically.
- Escape `@` as `&#64;` inside static markup (including `<pre>`) when needed.
- No page-level toolbar; controls are per block (global policy).
- See in-app Step 13 and Step 4 for the exact look and feel.

```html
<div class="bg-white rounded-xl p-5 sm:p-6 border border-gray-200">
  <!-- heading/description/bullets ... -->
  <div class="bg-gray-100 rounded-lg p-4 overflow-x-auto break-all">
    <pre data-code-title="Razor + C# (Component.razor)">
@* Paste code here as plain text. Use &#64; to show @ in static examples. *@
    </pre>
  </div>
</div>
```
