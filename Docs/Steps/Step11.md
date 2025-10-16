# Step 11 — Assignment Logic & Business Rules

- Live example: /examples/step11
- Next step: [Step 12 — Pay & Expense Calculation](./Step12.md)
- Previous: [Step 10 — State Management](./Step10.md)

What you'll do:

- Enforce domain rules in forms
- Validate inputs beyond annotations

Summary:

- Implement custom validation and business rules.
- Prevent invalid assignments at the UI layer.
- Surface friendly validation messages.

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
		</pre
    >
  </div>
</div>
```
