# Step 5: Lists & Loops

## Overview

This step teaches students how to render collections of data using loops in Blazor. Students learn about `@foreach`, `@for`, and best practices for rendering lists efficiently.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step5`)
- `Example.razor.cs` - Code-behind with sample data and logic
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step5`

## What Students Learn

1. Rendering lists with `@foreach` loops
2. Using `@key` directive for efficient rendering
3. Conditional rendering with `@if` statements
4. Working with List<T> and collections
5. Displaying empty states when no data exists

## Key Concepts

- **@foreach Loop** - Iterating over collections
- **@key Directive** - Optimizing list rendering
- **@if Statements** - Conditional rendering
- **LINQ Skip/Take** - Pagination basics

## Architecture

- **Tutorial Page**: Uses `ExampleBase` from code-behind
- **Sample Data**: Driver list with properties (Name, License, Experience)
- **Interactive Demo**: Shows real list rendering with styling

## Prerequisites

- Step 4: Data Binding & Events (handling state changes)

## Next Steps

- Step 6: Conditional Rendering & Styling (dynamic CSS classes)

## Code Structure

### Sample Data Model

```csharp
protected class SampleDriver
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string LicenseLevel { get; set; } = "";
    public int YearsOfExperience { get; set; }
}
```

### Collection Rendering

```razor
@foreach (var driver in drivers)
{
    <div @key="driver.Id" class="driver-card">
        <h3>@driver.Name</h3>
        <p>@driver.LicenseLevel - @driver.YearsOfExperience years</p>
    </div>
}
```

## Styling Patterns

- Grid layout for list items
- Card-based design for each item
- Empty state messaging when list is null or empty

## Common Issues & Solutions

### List Not Updating

**Issue**: Adding/removing items doesn't refresh the UI  
**Solution**: Ensure you're calling `StateHasChanged()` after modifying the list, or use proper event handlers that trigger re-rendering.

### Key Directive Errors

**Issue**: Warnings about duplicate keys or missing @key  
**Solution**: Always use `@key` with a unique identifier (like Id property). Never use index as key if items can be reordered.

### Null Reference Exceptions

**Issue**: Error when rendering empty or null lists  
**Solution**: Add null checks with `@if (list is not null)` before the foreach loop.

## Related Resources

- [Rendering Lists Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/rendering)
- [LINQ Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
