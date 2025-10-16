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
