# Tutorial Step Template

This document defines the standard structure, styling, and organization for all tutorial steps in the BlazorMock application.

## Folder Structure

Each tutorial step should be organized in its own folder:

```
Components/Pages/Examples/{Domain}/{StepN}/
├── Example.razor              # Main tutorial page (markup only)
├── Example.razor.cs           # Code-behind with logic for tutorial page
├── {DemoComponent}.razor      # Actual demo component students create (markup only)
├── {DemoComponent}.razor.cs   # Code-behind with logic for demo component
└── README.md                  # Documentation explaining the step folder
```

### Examples:

- `Components/Pages/Examples/Pokemon/Step2/`

  - `Example.razor` (tutorial page)
  - `Example.razor.cs` (code-behind for tutorial page)
  - `PokemonListExample.razor` (demo component)
  - `PokemonListExample.razor.cs` (code-behind for demo component)
  - `README.md` (documentation)

- `Components/Pages/Examples/Trucking/Step3/`
  - `Example.razor` (tutorial page)
  - `Example.razor.cs` (code-behind for tutorial page)
  - `GreetingCard.razor` (demo component)
  - `GreetingCard.razor.cs` (code-behind for demo component)
  - `README.md` (documentation)

## File Naming Convention

- **Tutorial Page**: Always `Example.razor` + `Example.razor.cs`
- **Demo Component**: Descriptive name like `PokemonListExample.razor` + `PokemonListExample.razor.cs`
- **Tutorial Code-behind class**: `ExampleBase` (not `PokemonStep2ExampleBase` or similar)
- **Demo Code-behind class**: `{ComponentName}Base` (e.g., `PokemonListExampleBase`, `GreetingCardBase`)

## Tutorial Page Structure (Example.razor)

### 1. Page Header

```razor
@page "/{domain}-examples/step{n}"
@rendermode InteractiveServer
@inherits ExampleBase

<PageTitle>{Domain} Step {N}: {Title} - Example</PageTitle>
```

### 2. Page Layout Sections (in order)

#### A. Header Section

```razor
<div class="min-h-screen py-8 rounded-4xl bg-gray-50">
    <div class="w-full mx-auto px-4 max-w-5xl">
        <!-- Back link -->
        <a href="/{domain}-guide" class="inline-flex items-center gap-2 text-sm sm:text-base text-blue-600 hover:text-blue-700 mb-4">
            <span>←</span> Back to {Domain} Guide
        </a>

        <!-- Title -->
        <h1 class="text-4xl font-bold text-gray-900 mb-4">
            🎯 Step {N}: {Title}
        </h1>

        <!-- Description -->
        <p class="text-xl text-gray-600">
            {Brief description of what this step teaches}
        </p>
    </div>
```

#### B. What You'll Learn Section (Gradient Banner)

```razor
<div class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 sm:p-6 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">What You'll Learn</h2>

    <!-- Description paragraph -->
    <p class="text-gray-700 mb-4 text-sm sm:text-base">
        {Detailed explanation of what students will learn}
    </p>

    <!-- Bullet points -->
    <ul class="list-disc ml-6 text-sm sm:text-base text-gray-700 space-y-2 mb-4">
        <li>{Learning point 1}</li>
        <li>{Learning point 2}</li>
        <li>{Learning point 3}</li>
        <li>{Learning point 4}</li>
    </ul>

    <!-- Key Concepts Footer -->
    <div class="mt-2 pt-3 border-t border-blue-200">
        <p class="text-xs sm:text-sm text-gray-700 mb-2">
            <strong>💡 Key Concepts:</strong> {concept1}, {concept2}, {concept3}
        </p>
        <div class="flex flex-wrap items-center gap-2">
            <span class="text-xs sm:text-sm text-gray-600">→ Related tips:</span>
            <a href="/tips#{topic}" class="px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-[11px] sm:text-xs font-medium hover:bg-blue-200 transition-colors">{Topic}</a>
        </div>
    </div>
</div>
```

#### C. Sample Component Section (White Card)

```razor
<div class="bg-white rounded-xl p-6 border border-gray-200 mb-6">
    <h2 class="text-xl font-bold text-gray-900 mb-4">🧩 Sample Component: {Component Name}</h2>

    <!-- Brief explanation -->
    <p class="text-sm text-gray-600 mb-4">
        {What this component demonstrates}
    </p>

    <!-- Full code example -->
    <div class="bg-gray-100 border border-gray-200 rounded-lg p-4 overflow-x-auto mb-4">
        <div class="text-xs text-gray-500 mb-1">{ComponentName}.razor</div>
        <p class="text-xs text-gray-600 mb-2">{What the component does}</p>
        <ul class="list-disc ml-4 mb-3 text-[11px] text-gray-600 space-y-1">
            <li>{Key point 1}</li>
            <li>{Key point 2}</li>
            <li>{Key point 3}</li>
        </ul>
        <pre data-code-title="Razor + C# ({ComponentName}.razor)"
            class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto">{Full component code}</pre>
    </div>

    <!-- Step-by-step breakdown -->
    <h3 class="text-sm font-semibold text-gray-800 mb-2">Step-by-step breakdown</h3>
    <div class="grid gap-4">
        <div class="bg-gray-100 border border-gray-200 rounded-lg p-4">
            <div class="text-xs text-gray-500 mb-1">1. {Breakdown title}</div>
            <p class="text-xs text-gray-600 mb-2">{Explanation}</p>
            <ul class="list-disc ml-4 mb-2 text-[11px] text-gray-600 space-y-1">
                <li>{Detail 1}</li>
                <li>{Detail 2}</li>
            </ul>
            <pre class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto">{Code snippet}</pre>
        </div>
        <!-- Repeat for each breakdown section -->
    </div>
</div>
```

#### D. Important Notes & Tips Section (White Card)

```razor
<div class="bg-white rounded-xl p-6 border border-gray-200 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">📝 Important Notes & Tips</h2>

    <div class="space-y-3">
        <!-- Pro Tips (Blue) -->
        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
            <h3 class="text-sm font-bold text-blue-900 mb-3">💡 Pro Tips</h3>
            <div class="space-y-4">
                <div>
                    <p class="text-sm font-semibold text-blue-900 mb-2">{Tip title}</p>
                    <p class="text-xs text-blue-800">{Detailed explanation with context and benefits}</p>
                </div>
                <!-- Repeat for each tip -->
            </div>
        </div>

        <!-- Troubleshooting (Yellow) -->
        <div class="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
            <h3 class="text-sm font-bold text-yellow-900 mb-2">⚠️ Troubleshooting</h3>
            <ul class="list-disc ml-6 text-sm text-yellow-900 space-y-1">
                <li>{Common issue 1 with solution}</li>
                <li>{Common issue 2 with solution}</li>
                <li>{Common issue 3 with solution}</li>
            </ul>
        </div>
    </div>
</div>
```

#### E. How to do it Section (Gradient Banner)

```razor
<div class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 sm:p-6 mb-6">
    <h2 class="text-xl sm:text-2xl font-semibold text-gray-800 mb-4">🛠️ How to do it</h2>
    <ol class="list-decimal ml-5 space-y-2 text-gray-700 text-sm sm:text-base">
        <li>
            <strong>{Step title}:</strong>
            {Detailed instruction with file path}
            <code>Components/Pages/Examples/{Domain}/Step{N}</code>
        </li>
        <li>
            <strong>{Step title}:</strong>
            {Detailed instruction}
        </li>
        <li>
            <strong>{Step title}:</strong>
            {Detailed instruction with route like} <code>/{domain}-examples/demo-{feature}</code>
        </li>
    </ol>
</div>
```

#### F. Live Demo Section (Optional)

```razor
<div class="bg-black/3 rounded-2xl p-6 mb-6">
    <h2 class="text-2xl font-semibold text-gray-800 mb-4">🎬 Live Demo</h2>
    <p class="text-sm text-gray-600 mb-4">{What the demo shows}</p>

    <div class="bg-white rounded-2xl border border-gray-200 p-6">
        @if ({data} is null)
        {
            <p class="text-sm text-gray-500">Loading...</p>
        }
        else
        {
            {Actual working demo markup}
        }
    </div>

    <p class="text-xs text-gray-600 mt-4 text-center">💡 {Helpful note about the demo pattern}</p>
</div>
```

#### G. Mark Complete Section (Green Card)

```razor
<div class="bg-white rounded-xl p-6 border-2 border-green-200 mb-6">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
            <h3 class="text-lg font-semibold text-gray-900 mb-1">Finished Step {N}?</h3>
            <p class="text-sm text-gray-600">{Completion message}</p>
        </div>
        @if (isComplete)
        {
            <div class="flex items-center gap-3">
                <span class="px-4 py-2 rounded-full bg-green-100 text-green-700 font-medium">✓ Completed</span>
                <button @onclick="ResetStep"
                    class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors text-sm">Reset</button>
            </div>
        }
        else
        {
            <button @onclick="MarkComplete"
                class="px-6 py-3 bg-green-600 hover:bg-green-700 text-white rounded-lg font-medium transition-colors">Mark as Complete</button>
        }
    </div>
</div>
```

#### H. Navigation Section

```razor
<div class="flex items-center justify-between">
    <a href="/{domain}-examples/step{n-1}"
        class="inline-flex items-center gap-2 px-4 py-2 border-2 border-gray-200 rounded-lg hover:bg-gray-50 transition-colors">
        <span>←</span>
        Previous: Step {N-1}
    </a>
    <a href="/{domain}-examples/step{n+1}"
        class="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors">
        Next: Step {N+1}
        <span>→</span>
    </a>
</div>
```

## Code-Behind Structure (Example.razor.cs)

```csharp
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.{Domain}.Step{N};

public partial class ExampleBase : ComponentBase, IDisposable
{
    // Dependency Injection
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    // State
    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
    protected List<{YourDataType}>? {yourData};

    // Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("{domain}", {N});
        isComplete = step?.IsComplete ?? false;

        // Load demo data if needed
        try
        {
            // Your data loading logic
        }
        catch
        {
            {yourData} = new List<{YourDataType}>();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _copyModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/codeblocks.js");
                await _copyModule.InvokeVoidAsync("enhancePreBlocks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load code block enhancer: {ex.Message}");
            }
        }
    }

    // Event Handlers
    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("{domain}", {N});
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("{domain}", {N});
        isComplete = false;
    }

    // Helper Methods
    protected static {ReturnType} {HelperMethod}({params})
    {
        // Helper logic
    }

    // Cleanup
    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }

    // DTOs and Inner Classes (if needed for live demo)
    protected sealed class {DtoName}
    {
        public {Type} {Property} { get; set; } = {default};
    }
}
```

## Demo Component Structure

Demo components should be simple, standalone examples with code-behind separation:

### Demo Component Markup ({DemoComponent}.razor)

```razor
@page "/{domain}-examples/demo-{feature}"
@rendermode InteractiveServer
@inherits {DemoComponent}Base

<h3 class="text-xl font-semibold mb-3">{Component Title}</h3>

@if ({data} is null)
{
    <p class="text-gray-500">Loading...</p>
}
else
{
    {Your component markup}
}
```

### Demo Component Code-Behind ({DemoComponent}.razor.cs)

```csharp
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.{Domain}.Step{N};

public partial class {DemoComponent}Base : ComponentBase
{
    // Dependency Injection
    [Inject] protected {RequiredService} {ServiceName} { get; set; } = default!;

    // State
    protected {Type}? {data};

    // Lifecycle
    protected override async Task OnInitializedAsync()
    {
        // Component logic
    }

    // Helper methods (if needed)
    protected {ReturnType} {MethodName}({params})
    {
        // Logic
    }

    // Models/DTOs specific to this component
    protected class {ModelName}
    {
        public {Type} {Property} { get; set; } = {default};
    }
}
```

## Styling Guidelines

### Colors & Themes

- **Gradient Banners**: `bg-gradient-to-r from-gray-100 to-blue-100`
- **White Cards**: `bg-white rounded-xl p-6 border border-gray-200`
- **Code Blocks**: `bg-gray-100 border border-gray-200 rounded-lg p-4`
- **Pro Tips**: `bg-blue-50 border border-blue-200` (blue theme)
- **Troubleshooting**: `bg-yellow-50 border border-yellow-200` (yellow theme)
- **Completion**: `bg-white border-2 border-green-200` (green theme)

### Typography

- **Page Title**: `text-4xl font-bold text-gray-900`
- **Section Headings**: `text-xl sm:text-2xl font-semibold text-gray-800`
- **Subsection Headings**: `text-sm font-semibold text-gray-800`
- **Body Text**: `text-sm sm:text-base text-gray-700`
- **Code Labels**: `text-xs text-gray-500`
- **Code Descriptions**: `text-xs text-gray-600`

### Spacing

- **Section Margins**: `mb-6` between major sections
- **Inner Spacing**: `space-y-3` or `space-y-4` for stacked items
- **List Spacing**: `space-y-1` or `space-y-2`

### Code Blocks

Always include:

- `data-code-title` attribute with file path
- `class="code-block"` for JS enhancement
- Responsive text sizing: `text-xs sm:text-sm`
- Overflow handling: `overflow-x-auto`

## Route Patterns

- Tutorial pages: `/{domain}-examples/step{n}`
- Demo components: `/{domain}-examples/demo-{feature}`
- Guides: `/{domain}-guide`

Examples:

- `/pokemon-examples/step2`
- `/pokemon-examples/demo-list`
- `/pokemon-guide`
- `/trucking-examples/step3`
- `/guide` (main trucking guide)

## Key Principles

1. **Consistency**: All steps should follow the same visual and structural pattern
2. **Clarity**: Every code block needs a title, description, and bullet points explaining what it does
3. **Self-Contained**: Each step folder contains everything related to that step
4. **Progressive**: Steps build on each other, with clear navigation between them
5. **Educational**: Focus on teaching concepts with real, working examples
6. **Accessible**: Use semantic HTML and proper heading hierarchy
7. **Responsive**: All layouts should work on mobile and desktop

## Checklist for New Tutorial Steps

- [ ] Folder created: `Components/Pages/Examples/{Domain}/Step{N}/`
- [ ] Files named correctly: `Example.razor`, `Example.razor.cs`, `{Demo}.razor`, `{Demo}.razor.cs`
- [ ] `README.md` created in step folder with documentation
- [ ] Route follows pattern: `/{domain}-examples/step{n}`
- [ ] Demo route follows pattern: `/{domain}-examples/demo-{feature}`
- [ ] Tutorial page uses `@inherits ExampleBase`
- [ ] Demo component uses `@inherits {DemoComponent}Base`
- [ ] Both components have `@rendermode InteractiveServer`
- [ ] Gradient "What You'll Learn" banner with description, bullets, and Key Concepts
- [ ] Sample component card with full code, description, and breakdown
- [ ] "Important Notes & Tips" section with Pro Tips (blue) and Troubleshooting (yellow)
- [ ] Gradient "How to do it" section with numbered steps
- [ ] Live demo section (if applicable)
- [ ] Completion tracking section (green card)
- [ ] Navigation links to previous/next steps
- [ ] All code blocks have `data-code-title` attributes
- [ ] All code examples have explanatory text and bullets
- [ ] Build succeeds without errors

## Step Folder README Template

Each step folder should include a `README.md` file with:

### Required Sections

1. **Overview** - Brief description of the step
2. **Files in This Folder** - List of all files with their purpose
3. **Routes** - Tutorial page and demo component URLs
4. **What Students Learn** - Numbered list of learning objectives
5. **Key Concepts** - Core concepts covered
6. **Architecture** - Code-behind pattern, DI, inheritance
7. **Prerequisites** - Required prior steps
8. **Next Steps** - What comes after this step
9. **Code Structure** - High-level class structure
10. **Styling Patterns** - CSS classes used
11. **Common Issues & Solutions** - Troubleshooting guide
12. **Related Resources** - External documentation links
