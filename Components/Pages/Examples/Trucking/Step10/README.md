# Step 10: State Management

## Overview

This step teaches students how to share state between components using services and events in Blazor. Students learn about service lifetimes, event-driven updates, and proper state management patterns.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step10`)
- `Example.razor.cs` - Code-behind with progress tracking and state demo
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step10`

## What Students Learn

1. Creating a state management service (AppState)
2. Understanding service lifetimes (Scoped, Singleton, Transient)
3. Using events (Action) to notify components of state changes
4. Subscribing to state changes in components
5. Implementing IDisposable to clean up event subscriptions

## Key Concepts

- **Service Lifetimes** - Scoped (per-user), Singleton (app-wide), Transient (per-request)
- **StateHasChanged()** - Manually trigger component re-rendering
- **Event Notifications** - Using C# events to communicate state changes
- **IDisposable** - Cleaning up resources and event subscriptions

## Architecture

- **AppState Service**: Centralized state with `SelectedDriver` property
- **Event Pattern**: `Action? OnChange` event for notifications
- **Service Registration**: Scoped lifetime in Program.cs
- **Component Lifecycle**: Subscribe in OnInitialized, unsubscribe in Dispose

## Prerequisites

- Step 9: CRUD Operations (understanding of data operations)

## Next Steps

- Step 11: Assignment Logic & Business Rules (validation and business logic)

## Code Structure

### AppState Service

```csharp
public class AppState
{
    private Driver? _selectedDriver;

    public Driver? SelectedDriver
    {
        get => _selectedDriver;
        private set
        {
            _selectedDriver = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    public void SelectDriver(Driver? driver) => SelectedDriver = driver;
    public void ClearSelection() => SelectedDriver = null;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
```

### Component Subscription

```csharp
@inject AppState AppState
@implements IDisposable

protected override void OnInitialized()
{
    AppState.OnChange += StateHasChanged;
}

public void Dispose()
{
    AppState.OnChange -= StateHasChanged;
}
```

### Service Registration

```csharp
// Program.cs
builder.Services.AddScoped<AppState>();
```

## Live Demo Features

The tutorial includes interactive components showing:

- DriverPicker component with dropdown selection
- SelectedDriverCard component displaying current selection
- Real-time updates when selection changes
- Clear button to reset selection

## Styling Patterns

- Service lifetime comparison cards (Scoped, Singleton, Transient)
- Code examples for state management patterns
- Live demo with two components sharing state
- Warning messages about memory leaks

## Common Issues & Solutions

### Component Doesn't Update

**Issue**: Component doesn't re-render when state changes  
**Solution**: Ensure you're calling `StateHasChanged()` in your event handler and that you subscribed to the OnChange event.

### State Shared Between Users

**Issue**: Different users see each other's state  
**Solution**: Use Scoped lifetime, not Singleton. Scoped creates one instance per user connection.

### Memory Leaks

**Issue**: Components stay in memory after disposal  
**Solution**: Always implement IDisposable and unsubscribe from events in the Dispose() method.

### "Disposed Object" Errors

**Issue**: Error accessing disposed service  
**Solution**: Ensure you're using Scoped lifetime for per-user services and not accessing the service after component disposal.

## Related Resources

- [Blazor State Management](https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management)
- [Dependency Injection in Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection)
- [Component Lifecycle](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle)
- [IDisposable Pattern](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose)
