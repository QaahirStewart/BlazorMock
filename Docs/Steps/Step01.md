# Step 01 — New Clean Project

- Live example: /examples/step1
- Next step: [Step 02 — Razor Syntax & Display](./Step02.md)
- Previous: —

What you'll do:

- Create a new Blazor project with interactive server mode
- Install Tailwind CSS v4 and set up input/output CSS
- Link compiled CSS in `Components/App.razor`

Commands:

- dotnet new blazor -o BlazorAppName --interactivity Server --all-interactive --empty
- npm install tailwindcss @tailwindcss/cli
- npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css --watch

Verify:

- `wwwroot/tailwind.css` is generated
- `<link rel="stylesheet" href="@Assets["tailwind.css"]" />` exists in App.razor
