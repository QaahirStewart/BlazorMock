# Step 00 — Prerequisites & VS Code Setup

- Live example: — (docs-only)
- Next step: [Step 01 — New Clean Project](./Step01.md)
- Previous: —

What you'll do:

- Install required tools (VS Code, .NET SDK, Node.js)
- Add helpful VS Code extensions
- Verify your environment works

Summary:

- Set up the toolchain and confirm everything runs locally. If any check fails, install the missing tool and ensure it’s on PATH.

## 1) Install Required Tools

- .NET SDK (matches this repo): .NET 10 SDK or later
- Node.js LTS (for Tailwind CLI)
- Git (optional, recommended)
- Visual Studio Code

## 2) Recommended VS Code Extensions

- C# (official)
- C# Dev Kit (optional)
- Razor Language
- Tailwind CSS IntelliSense

## 3) Verify Environment

Run these in VS Code terminal (PowerShell):

<details>
	<summary>Show code — Verification commands</summary>

```pwsh
dotnet --info
node --version
npm --version
```

</details>

You should see versions without errors. If any command isn't found, install or add to PATH.

## 4) Create a Workspace Folder

- Create or open a folder where you’ll build your Trucking Schedule App
- From here, continue with Step 01 to scaffold the Blazor project and Tailwind

Next: Go to [Step 01 — New Clean Project](./Step01.md)

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
