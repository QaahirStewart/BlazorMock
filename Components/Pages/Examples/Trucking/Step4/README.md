# Step 4: Data Binding & Events

## Overview

This step teaches students how to handle user input through data binding and event handlers in Blazor. Students learn the difference between one-way and two-way binding, and how to respond to user interactions.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step4`)
- `Example.razor.cs` - Code-behind with progress tracking and demo state
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step4`

## What Students Learn

1. One-way binding with `@` syntax
2. Two-way binding with `@bind` directive
3. Event handling with `@onclick`, `@onchange`, etc.
4. Lambda expressions for inline event handlers
5. State management within a component

## Key Concepts

- **Data Binding** - Connecting UI to component properties
- **Event Handlers** - Responding to user interactions
- **@bind Directive** - Two-way data binding shorthand
- **Lambda Expressions** - Inline anonymous methods

## Architecture

- **Tutorial Page**: Uses `ExampleBase` from code-behind
- **Render Mode**: `InteractiveServer` for reactivity
- **DI Services**: `ILearningProgressService`, `IJSRuntime`

## Prerequisites

- Step 3: Components & Parameters (component basics)

## Next Steps

- Step 5: Lists & Loops (rendering collections)

## Code Structure

### Demo State

```csharp
protected string inputText = "";
protected int counter = 0;
protected bool isChecked = false;
```

### Event Handlers

```csharp
protected void IncrementCounter() => counter++;
protected void ResetDemo()
{
    inputText = "";
    counter = 0;
    isChecked = false;
}
```

## Styling Patterns

- Live demo section with interactive inputs
- Real-time feedback showing bound values
- Clean form styling with Tailwind utilities

## Common Issues & Solutions

### Binding Not Working

**Issue**: Input changes don't update the component property  
**Solution**: Use `@bind` for two-way binding, not just `value="@property"`. For custom events, use `@bind:event="oninput"`.

### Event Handler Not Firing

**Issue**: Click events don't trigger methods  
**Solution**: Verify method signature is correct (no parameters for simple events) and the method name matches exactly.

## Related Resources

- [Data Binding Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/data-binding)
- [Event Handling](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/event-handling)
