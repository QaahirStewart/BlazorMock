# Step 6: Conditional Rendering & Styling

## Overview

This step teaches students how to apply conditional logic to rendering and styling in Blazor. Students learn to dynamically change CSS classes, show/hide elements, and create responsive UIs based on state.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step6`)
- `Example.razor.cs` - Code-behind with demo state and helper methods
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step6`

## What Students Learn

1. Conditional rendering with `@if`, `@else if`, `@else`
2. Dynamic CSS classes based on state
3. Using ternary operators for inline conditions
4. Helper methods for calculating CSS classes
5. Switch expressions for multi-condition styling

## Key Concepts

- **Conditional Rendering** - Show/hide elements based on state
- **Dynamic CSS Classes** - Change styling based on data
- **Ternary Operators** - Inline conditional expressions
- **Switch Expressions** - Pattern matching for multiple conditions

## Architecture

- **Tutorial Page**: Uses `ExampleBase` from code-behind
- **Demo State**: Status tracking, availability flags
- **Helper Methods**: CSS class calculators

## Prerequisites

- Step 5: Lists & Loops (working with collections)

## Next Steps

- Step 7: EF Core Models (defining data entities)

## Code Structure

### Helper Methods

```csharp
protected string GetStatusClass(string status)
{
    return status switch
    {
        "Available" => "bg-green-100 text-green-800",
        "On Route" => "bg-blue-100 text-blue-800",
        "Maintenance" => "bg-yellow-100 text-yellow-800",
        _ => "bg-gray-100 text-gray-800"
    };
}
```

### Conditional Rendering

```razor
@if (driver.IsAvailable)
{
    <span class="badge badge-success">Available</span>
}
else
{
    <span class="badge badge-warning">Unavailable</span>
}
```

## Styling Patterns

- Status badges with color-coded backgrounds
- Conditional icon display
- Dynamic border colors and text styles
- Responsive visibility based on screen size

## Common Issues & Solutions

### CSS Classes Not Applying

**Issue**: Dynamic classes don't change the appearance  
**Solution**: Verify Tailwind CSS is processing the classes. Use complete class names, not string concatenation (Tailwind needs to see full class names at build time).

### Condition Always True/False

**Issue**: UI doesn't update when state changes  
**Solution**: Ensure the condition is checking the correct property and StateHasChanged() is called after state updates.

### Switch Expression Errors

**Issue**: Syntax errors in switch expressions  
**Solution**: Use `=>` (not `:`) for cases and ensure all paths return the same type. Include a default case with `_`.

## Related Resources

- [Conditional Rendering Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/rendering)
- [C# Switch Expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
