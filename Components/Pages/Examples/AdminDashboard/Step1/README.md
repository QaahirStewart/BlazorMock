# Admin Dashboard Step 1 — New Clean Project

## Overview

This step walks you through creating a brand new Blazor Server project for the Admin Dashboard guide and wiring up Tailwind CSS v4 so you have a modern styling foundation.

## Files in This Folder

- `Example.razor` — Tutorial page explaining the step and showing commands.
- `Example.razor.cs` — Code-behind for the tutorial page with progress tracking.

## Routes

- Tutorial page: `/admin-dashboard-examples/step1`

## What Students Learn

1. How to create a new Blazor Server project using the .NET CLI.
2. How to install Tailwind CSS v4 and its CLI via npm.
3. How to set up a simple Tailwind input file and compile it.
4. How to connect the compiled CSS to the Blazor app via the asset pipeline.

## Key Concepts

- `dotnet new blazor` template
- CLI flags: `--interactivity`, `--all-interactive`, `--empty`
- Tailwind CSS v4 and `@@import "tailwindcss";`
- Asset pipeline with `@@Assets["tailwind.css"]`

## Architecture

- Tutorial page inherits `ExampleBase` in the `AdminDashboard.Components.Pages.Examples.AdminDashboard.Step1` namespace.
- Uses `ILearningProgressService` to track completion for the `admin-dashboard` domain, step `1`.
- Uses `IJSRuntime` to load `codeblocks.js` and enhance `<pre class="code-block">` elements.

## Prerequisites

- Step 0 (Phase 1 guide): prerequisites & VS Code setup, including installing .NET SDK and VS Code.

## Next Steps

- Proceed to Step 2 to continue setting up the Admin Dashboard experience.

## Code Structure

- `ExampleBase` handles:
  - Loading completion state from `ILearningProgressService`.
  - Marking the step complete/incomplete.
  - Initializing JS module for code block enhancements.

## Styling Patterns

- Uses gradient banners (`bg-gradient-to-r from-gray-100 to-blue-100`) for "What You'll Learn" and "How to do it".
- Uses white cards with borders for command/code sections.
- Uses consistent typography and spacing per `TUTORIAL_TEMPLATE.md`.

## Common Issues & Solutions

- **Tailwind CLI not found**: Ensure `npm install tailwindcss @@tailwindcss/cli` completed successfully and try `npx @@tailwindcss/cli --help`.
- **No styles appearing**: Verify Tailwind build command ran and `wwwroot/tailwind.css` exists. Ensure the `<link>` tag using `@@Assets["tailwind.css"]` is inside the `<head>` of `Components/App.razor`.
- **CSS not updating**: Make sure Tailwind is running with `--watch` and refresh the browser with a hard reload.

## Related Resources

- .NET CLI docs: https://learn.microsoft.com/dotnet/core/tools/
- Tailwind CSS v4 docs: https://tailwindcss.com/docs
