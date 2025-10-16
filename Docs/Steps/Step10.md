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
