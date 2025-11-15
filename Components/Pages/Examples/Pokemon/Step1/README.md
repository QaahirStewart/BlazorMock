# Pokemon Step 1: New Clean Project

## Overview

This step teaches students how to create a new Blazor Server application from scratch and set up Tailwind CSS v4 for styling. This is a foundational configuration step that establishes the development environment for the Pokemon Collector app.

## Files in This Folder

- **Example.razor** - Tutorial page teaching project creation and Tailwind setup (markup only)
- **Example.razor.cs** - Code-behind with progress tracking and JS interop logic
- **README.md** - This documentation file

Note: Step 1 is a configuration/setup step with no demo component since it teaches CLI commands and project initialization.

## Routes

- Tutorial page: `/pokemon-examples/step1`

## What Students Learn

1. How to create a new Blazor Server project using dotnet CLI
2. Understanding CLI flags (`--interactivity Server`, `--all-interactive`, `--empty`)
3. Installing Tailwind CSS v4 via npm
4. Setting up Tailwind CSS build process with watch mode
5. Linking compiled CSS in the App.razor file
6. Working with the .NET asset pipeline

## Key Concepts

- **dotnet CLI**: Command-line interface for creating .NET projects
- **npm package management**: Installing JavaScript packages like Tailwind CSS
- **Tailwind CSS v4**: Modern utility-first CSS framework with simplified configuration
- **Asset pipeline**: How .NET resolves and serves static assets like CSS files
- **Watch mode**: Automatic rebuilds when source files change

## Architecture

This step follows the standard tutorial page pattern:

### Code-Behind Separation

- **Example.razor**: Contains only markup and UI structure
- **ExampleBase class**: Inherits from `ComponentBase`, contains all logic:
  - Progress tracking via `ILearningProgressService`
  - Navigation via `NavigationManager`
  - JavaScript interop for code block enhancement via `IJSRuntime`

### Dependency Injection

Uses `[Inject]` attributes in code-behind:

```csharp
[Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
[Inject] protected NavigationManager Navigation { get; set; } = default!;
[Inject] protected IJSRuntime JS { get; set; } = default!;
```

### Component Inheritance

Razor file uses `@inherits ExampleBase` to connect markup to code-behind logic.

## Prerequisites

- None - this is the first step in the Pokemon tutorial series
- Requires .NET SDK and Node.js/npm installed on the development machine

## Next Steps

After completing this step, students proceed to:

- **Step 0**: Configuring HttpClient for PokeAPI integration
- **Step 2**: Making API calls and rendering Pokemon lists

## Code Structure

### Example.razor.cs

```csharp
namespace BlazorMock.Components.Pages.Examples.Pokemon.Step1;

public partial class ExampleBase : ComponentBase, IDisposable
{
    // Services via DI
    [Inject] protected ILearningProgressService ProgressService { get; set; }
    [Inject] protected NavigationManager Navigation { get; set; }
    [Inject] protected IJSRuntime JS { get; set; }

    // State
    protected bool isComplete;
    protected IJSObjectReference? _copyModule;

    // Lifecycle: Load completion status
    protected override async Task OnInitializedAsync()

    // Lifecycle: Enhance code blocks with JS
    protected override async Task OnAfterRenderAsync(bool firstRender)

    // Actions: Mark step complete/incomplete
    protected async Task MarkComplete()
    protected async Task ResetStep()

    // Cleanup
    public void Dispose()
}
```

## Page Sections

1. **Header**: Title, description, back link to Pokemon guide
2. **What You'll Do**: Step-by-step instructions for project setup
   - Create Blazor project with CLI
   - Install Tailwind CSS via npm
   - Create Styles folder and input.css
   - Build Tailwind with watch mode
   - Link CSS in App.razor
3. **How to do it**: Summary of the three main tasks
4. **Mark Complete**: Green card with completion tracking
5. **Navigation**: Links to Pokemon guide and Step 2

## Styling Patterns

- **White cards**: `bg-white rounded-xl p-5 sm:p-6 border border-gray-200`
- **Code blocks**: `bg-gray-100 p-3 sm:p-4 rounded text-xs sm:text-sm font-mono`
- **Info boxes (blue)**: `bg-blue-50 border border-blue-200 rounded-lg`
- **Warning boxes (yellow)**: `bg-yellow-50 border border-yellow-200 rounded-lg`
- **Gradient banner**: `bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl`
- **Completion card**: `bg-white rounded-xl border-2 border-green-200`

All code blocks include `data-code-title` attributes for JS enhancement.

## CLI Commands Taught

### Create Blazor Project

```bash
dotnet new blazor -o PokemonCollector --interactivity Server --all-interactive --empty
```

Flags explained:

- `-o PokemonCollector`: Output directory name
- `--interactivity Server`: Enable server-side interactivity
- `--all-interactive`: All components interactive by default
- `--empty`: Clean template without sample pages

### Install Tailwind CSS

```bash
npm install tailwindcss @tailwindcss/cli
```

### Build Tailwind with Watch Mode

```bash
npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css --watch
```

The `--watch` flag keeps the build process running and automatically rebuilds when input.css changes.

## Common Issues & Solutions

**Issue**: Tailwind CLI not found after npm install  
**Solution**: Make sure npm install completed successfully. Try `npx @tailwindcss/cli --help` to verify installation.

**Issue**: Styles not appearing in the app  
**Solution**: Verify the `<link>` tag is in App.razor's `<head>` section and that the Tailwind build process is running.

**Issue**: CSS not updating when making changes  
**Solution**: Ensure the Tailwind CLI is running with `--watch` flag. Check the terminal for build errors.

**Issue**: Asset not found error  
**Solution**: Verify `tailwind.css` exists in `wwwroot/` folder after running the build command.

**Issue**: Build error "The type or namespace name 'ExampleBase' could not be found"  
**Solution**: After moving files to the Step1 folder, an orphaned copy may remain at `Components/Pages/Examples/PokemonStep1Example.razor`. Delete this old file:

```bash
Remove-Item -Path "d:\BlazorMock\Components\Pages\Examples\PokemonStep1Example.razor" -Force
```

## Related Resources

- [Blazor Server Overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models#blazor-server)
- [Tailwind CSS v4 Documentation](https://tailwindcss.com/docs)
- [.NET CLI Reference](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [npm Documentation](https://docs.npmjs.com/)
