# Trucking Step 1: New Clean Project

## Overview

This step teaches students how to create a new Blazor Server application from scratch using the .NET CLI and set up Tailwind CSS v4 for modern styling. It focuses on project initialization, tooling, and asset pipeline configuration.

## Files in This Folder

- **Example.razor** - Tutorial page (markup only, route: `/trucking-examples/step1`)
- **Example.razor.cs** - Code-behind with completion tracking logic (`ExampleBase` class)

**Note**: This step has no demo component since it covers project setup commands rather than component creation.

## Routes

- **Tutorial**: `/trucking-examples/step1`
- **Back to Guide**: `/trucking-guide`
- **Next Step**: `/trucking-examples/step2`

## What Students Learn

1. How to use the `dotnet` CLI to create a new Blazor Server project with custom flags
2. How to install npm packages (Tailwind CSS v4 and its CLI)
3. How to set up Tailwind's build process with watch mode for live CSS updates
4. How to integrate compiled CSS into Blazor using the asset pipeline
5. Understanding the difference between source files (`Styles/input.css`) and output files (`wwwroot/tailwind.css`)

## Key Concepts

- **dotnet CLI**: Command-line tool for .NET project management
- **npm packages**: JavaScript package manager for installing tools like Tailwind
- **Tailwind CSS v4**: Utility-first CSS framework with simplified setup
- **.NET Asset Pipeline**: Fingerprinting system for cache busting static files

## Architecture

### Code-Behind Pattern

- Uses `@inherits ExampleBase` in the Razor file
- Code-behind class named `ExampleBase` (not `TruckingStep1ExampleBase`)
- Implements `IDisposable` for proper cleanup of JS interop modules

### Dependencies Injected

- `ILearningProgressService` - Tracks step completion
- `NavigationManager` - For navigation (if needed in future)
- `IJSRuntime` - For code block copy enhancement

### State Management

- `isComplete` - Boolean tracking whether student marked this step done
- `_copyModule` - JS interop reference for code block enhancements

## Prerequisites

Students should have:

- .NET SDK installed (version 8.0 or later)
- Node.js and npm installed (for Tailwind CSS)
- Basic familiarity with command-line terminals
- Text editor or IDE (VS Code, Visual Studio, Rider, etc.)

## Next Steps

After completing this step, students will:

- Have a clean Blazor Server project ready for development
- Have Tailwind CSS configured and running in watch mode
- Be ready to start building components in Step 2

## Styling Patterns Used

### Template Compliance

- ✅ Gradient banner for "What You'll Learn" (`bg-gradient-to-r from-gray-100 to-blue-100`)
- ✅ White cards with rounded corners for content sections
- ✅ Code blocks with `data-code-title` attributes and `class="code-block"` for JS enhancement
- ✅ Each code example has: label → description → bullet points → code block
- ✅ Pro Tips (blue theme: `bg-blue-50 border border-blue-200`)
- ✅ Troubleshooting (yellow theme: `bg-yellow-50 border border-yellow-200`)
- ✅ Completion tracking (green theme: `border-2 border-green-200`)
- ✅ Responsive typography (`text-xs sm:text-sm`, `text-sm sm:text-base`)
- ✅ All @ symbols properly escaped as @@ in code examples and text

### Typography

- Page title: `text-4xl` (fixed, no responsive variants per template)
- Page description: `text-xl` (fixed, no responsive variants per template)
- Section headings: `text-xl sm:text-2xl`
- Code labels: `text-xs text-gray-500`
- Code descriptions: `text-xs text-gray-600`
- Code bullets: `text-[11px] text-gray-600`

## Common Issues & Solutions

### Issue: "npm: command not found"

**Cause**: Node.js is not installed or not in PATH.

**Solution**: Download and install Node.js from [nodejs.org](https://nodejs.org). Restart your terminal after installation.

### Issue: Tailwind classes not applying to elements

**Cause**: The Tailwind CLI may not be running, or the output file wasn't created.

**Solution**:

1. Check if `wwwroot/tailwind.css` exists
2. Ensure the CLI is running with `--watch` flag
3. Look for errors in the Tailwind CLI terminal
4. Hard refresh your browser (Ctrl+Shift+R)

### Issue: CSS changes not reflecting in browser

**Cause**: Browser is caching the old CSS file.

**Solution**:

- The asset pipeline should handle this with fingerprinting
- Try hard refresh (Ctrl+Shift+R or Cmd+Shift+R)
- Check that you're using `@Assets["tailwind.css"]` not a hardcoded path

### Issue: "The type or namespace name 'ExampleBase' could not be found"

**Cause**: An orphaned old file exists at the original location causing namespace conflicts.

**Solution**: Delete the old `Step1Example.razor` file from `Components/Pages/Examples/` if it exists.

### Issue: Asset pipeline not generating fingerprinted URLs

**Cause**: File may not be in `wwwroot` folder or syntax is incorrect.

**Solution**:

- Verify `tailwind.css` is in `wwwroot` (not `Styles`)
- Ensure you're using `@Assets["tailwind.css"]` with @ properly escaped in documentation
- Check App.razor has the correct `<link>` tag in the `<head>` section

## Teaching Notes

### Emphasize Understanding Over Memorization

Students should understand **why** each flag is used, not just copy-paste commands:

- `--interactivity Server` → Explains the rendering mode (server-side with WebSocket)
- `--all-interactive` → All components interactive by default (can override per-component)
- `--empty` → Clean slate without sample pages (better for learning)
- `--watch` → Live reload during development (essential workflow)

### Explain the Build Process

Help students visualize the Tailwind workflow:

1. You write HTML with Tailwind classes (`bg-blue-100`, `p-4`, etc.)
2. Tailwind CLI scans your files for those classes
3. It generates CSS containing only the classes you actually use
4. Output goes to `wwwroot/tailwind.css`
5. Blazor's asset pipeline fingerprints it for cache busting
6. Your app loads the final CSS with a hash in the URL

### Two Terminals Pattern

Students will need to run two separate terminals during development:

1. **Tailwind CLI**: `npx @tailwindcss/cli ... --watch` (keeps running)
2. **Blazor App**: `dotnet run` or `dotnet watch` (keeps running)

Make this clear early - it's a common source of confusion.

### Asset Pipeline Analogy

The asset pipeline is like a version number that updates automatically:

- Without it: `style.css` (browser caches, updates don't show)
- With it: `style.abc123.css` (hash changes when file changes, forces fresh download)

## Related Resources

- [.NET CLI Documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [Tailwind CSS v4 Documentation](https://tailwindcss.com/docs)
- [npm Documentation](https://docs.npmjs.com/)
- [Blazor Static Assets](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/static-files)

## Tips Created for This Step

This step introduced 4 new tips to the TipsService:

1. **dotnet CLI** (Tooling — .NET) - Command-line tool for .NET project management
2. **npm packages** (Tooling — Frontend) - JavaScript package manager
3. **Tailwind CSS** (Styling — CSS Frameworks) - Utility-first CSS framework
4. **.NET Asset Pipeline** (Blazor — Static Files) - Fingerprinting for cache busting

All tips are registered in Program.cs via `Step1TipsContributor` and linked from the tutorial's "Related tips" section.
