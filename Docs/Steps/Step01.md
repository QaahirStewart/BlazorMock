# Step 01 — New Clean Project

What you'll do:

- Create a new Blazor project with interactive server mode
- Install Tailwind CSS v4 and set up input/output CSS
- Link compiled CSS in `Components/App.razor`

Summary:

- Scaffold a minimal Blazor app with interactive server mode.
- Install Tailwind and wire up input/output CSS.
- Confirm the compiled CSS is referenced in `Components/App.razor`.

Commands:

<details>
	<summary>Show code — Scaffold and Tailwind setup</summary>

```pwsh
dotnet new blazor -o BlazorAppName --interactivity Server --all-interactive --empty
npm install tailwindcss @tailwindcss/cli
npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css --watch
```

</details>

Verify:

- `wwwroot/tailwind.css` is generated
- `<link rel="stylesheet" href="@Assets["tailwind.css"]" />` exists in App.razor

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
