# ELI5 (Explain Like I'm 5) — Blazor & C# Quick Tips

This file contains short ELI5 answers, examples, and tips for common Blazor/C# patterns. It's designed to be both human-readable and usable as a quick reference inside the app.

---

## private void vs private async Task

ELI5

- "private" = only this component can use the method.
- "void" = the method doesn't give anything back.

Example

```csharp
private int count = 0;
private void Increment() => count++;
```

Tip

- Use `private async Task MethodName()` if the method does asynchronous work (e.g., calls an API). Avoid `async void` except for event handlers in UI frameworks.

---

## @onclick and event binding

ELI5

- `@onclick` is how you tell a button "when clicked, run this little action".

Example

```razor
<button @onclick="Increment">+1</button>

@code {
    private int count = 0;
    private void Increment() => count++;
}
```

Tip

- Keep event handlers small and focused. If you need async work, return a `Task` and use `async` / `await`.

---

## private vs public methods

ELI5

- `private` = "my secret helper" inside the component.
- `public` = "I might let other code call this".

Tip

- Prefer `private` for component internals. Use `public` only when another component or service must call the method.

---

## Common nit: spelling mistakes (`viod` vs `void`)

ELI5

- The compiler can't read misspelled keywords. `viod` is not a word the compiler knows; use `void`.

Tip

- Use an editor with C# support (IntelliSense) to catch these early.

---

Add more entries as you ask questions — this file will grow with helpful ELI5 explanations, examples, and tips.
