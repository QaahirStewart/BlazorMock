# Step 3: Components & Parameters

## Overview

This step introduces Blazor components and component parameters. Students learn how to create reusable components and pass data between parent and child components using parameters.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step3`)
- `Example.razor.cs` - Code-behind with progress tracking logic
- `GreetingCard.razor` - Demo component showing component parameters
- `GreetingCard.razor.cs` - Code-behind for demo component
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step3`
- **Demo Component**: `/trucking-examples/demo-greeting`

## What Students Learn

1. How to create reusable Blazor components
2. Using `[Parameter]` attribute to accept data from parent components
3. Component composition and rendering child components
4. Passing different values to the same component to create variations
5. Basic component styling with Tailwind CSS classes

## Key Concepts

- **Components** - Reusable UI building blocks
- **Parameters** - Props/attributes passed from parent to child
- **Component Composition** - Building complex UIs from smaller pieces
- **Tailwind CSS** - Utility-first CSS framework for styling

## Architecture

- **Tutorial Page**: Uses `ExampleBase` from code-behind with `@inherits`
- **Demo Component**: Uses `GreetingCardBase` from code-behind with `@inherits`
- **Render Mode**: `InteractiveServer` for both pages
- **DI Services**: `ILearningProgressService` for completion tracking

## Prerequisites

Students should have completed:

- Step 1: Project Setup (project created and running)
- Step 2: Routing & Navigation (understanding of page routes)

## Next Steps

After completing this step, students move to:

- Step 4: Data Binding & Events (handling user interactions)

## Code Structure

### GreetingCard Component

```csharp
public partial class GreetingCardBase : ComponentBase
{
    [Parameter] public string Name { get; set; } = "Friend";
    [Parameter] public string Message { get; set; } = "Welcome!";
}
```

**Key Features**:

- Two string parameters with default values
- Simple, focused component demonstrating parameter basics
- Styled with Tailwind CSS for visual appeal

### Tutorial Page

```csharp
public partial class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
}
```

**Key Features**:

- Progress tracking with completion state
- JS interop for code block enhancement
- Proper cleanup with IDisposable

## Styling Patterns

- **Gradient Banner**: "What You'll Learn" section uses `bg-gradient-to-r from-gray-100 to-blue-100`
- **White Cards**: Code examples use `bg-white rounded-xl p-6 border border-gray-200`
- **Code Blocks**: `bg-gray-100 border border-gray-200 rounded-lg p-4`
- **Pro Tips**: Blue theme with `bg-blue-50 border border-blue-200`
- **Troubleshooting**: Yellow theme with `bg-yellow-50 border border-yellow-200`

## Common Issues & Solutions

### Component Not Rendering

**Issue**: GreetingCard component doesn't appear on the page  
**Solution**: Verify the namespace matches and the component is referenced correctly with its full namespace or via `@using` directive.

### Parameters Not Updating

**Issue**: Changing parameter values doesn't update the component  
**Solution**: Ensure parameter names match exactly (case-sensitive) and you're using the correct syntax: `<GreetingCard Name="John" Message="Hello!" />`

### Styling Not Applied

**Issue**: Tailwind CSS classes aren't working  
**Solution**: Verify Tailwind CSS is properly configured and the development server is running with `dotnet watch`.

## Related Resources

- [Blazor Components Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/)
- [Component Parameters](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/parameters)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
