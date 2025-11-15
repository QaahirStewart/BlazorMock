# Pokemon Step 4: Loading & Error States

## Overview

Step 4 teaches students how to handle loading, error, and empty states when fetching data asynchronously. This is a critical real-world skill—apps need to show spinners while loading, display friendly error messages when things fail, and handle edge cases like empty results gracefully.

## Files in This Folder

- **Example.razor** - Tutorial page (markup) teaching loading/error state patterns
- **Example.razor.cs** - Code-behind (ExampleBase) with live demo of try-catch-finally
- **PokemonLoadingExample.razor** - Demo component (markup) showing all three states
- **PokemonLoadingExample.razor.cs** - Code-behind (PokemonLoadingExampleBase) with robust async data handling
- **README.md** - This documentation file

## Routes

- **Tutorial Page**: `/pokemon-examples/step4`
- **Demo Component**: `/pokemon-examples/demo-loading`

## What Students Learn

1. **Loading State Pattern** - Show spinners/skeleton loaders while data fetches
2. **Error State Pattern** - Display user-friendly error messages with retry buttons
3. **Empty State Pattern** - Handle cases where API returns zero results
4. **try-catch-finally** - Robust error handling ensuring cleanup always happens
5. **StateHasChanged** - Force re-renders when calling lifecycle methods manually
6. **Conditional Rendering** - Use @if/else to show different UI for different states
7. **Tailwind Spinners** - Create CSS-only loading animations with animate-spin

## Key Concepts

### try-catch-finally Pattern

```csharp
try
{
    isLoading = true;
    errorMessage = null;  // Clear old errors
    data = await FetchAsync();
}
catch (HttpRequestException ex)
{
    errorMessage = $"Network error: {ex.Message}";
}
catch (Exception ex)
{
    errorMessage = "Something went wrong. Please try again.";
    Console.WriteLine(ex);  // Log for debugging
}
finally
{
    isLoading = false;  // ALWAYS runs, even if error occurred
}
```

### State Flags

- **isLoading**: Boolean tracking whether async operation is in progress

  - Initialize to `true` if data loads on component init
  - Initialize to `false` if data loads on user action
  - Set to `true` when starting async operation
  - Set to `false` in `finally` block

- **errorMessage**: Nullable string storing error text
  - `null` = no error
  - Has value = error occurred
  - Clear to `null` before retry attempts

### Conditional Rendering Order

Check states in this order for best UX:

1. `@if (isLoading)` - Show spinner (highest priority)
2. `else if (!string.IsNullOrEmpty(errorMessage))` - Show error card
3. `else if (data.Count == 0)` - Show empty state
4. `else` - Show actual data

### Retry Pattern

```csharp
protected async Task RetryLoad()
{
    await OnInitializedAsync();  // Re-run fetch logic
    StateHasChanged();  // Force Blazor to re-render
}
```

Why StateHasChanged is needed: When you manually call lifecycle methods like OnInitializedAsync, Blazor doesn't automatically trigger a re-render. You must explicitly tell it the state changed.

## Architecture

### Code-Behind Pattern

Both components use code-behind separation:

- **Example.razor.cs** extends `ComponentBase` with `ExampleBase` class
- **PokemonLoadingExample.razor.cs** extends `ComponentBase` with `PokemonLoadingExampleBase` class
- Markup files use `@inherits` to connect to their code-behind classes

### Dependency Injection

Tutorial page (`ExampleBase`) injects:

- `ILearningProgressService` - Track step completion
- `NavigationManager` - Navigate between steps
- `IJSRuntime` - Load code block enhancer
- `HttpClient` - Fetch Pokemon data for live demo

Demo component (`PokemonLoadingExampleBase`) injects:

- `HttpClient` - Fetch Pokemon data from PokeAPI

### State Management

Both components track three pieces of state:

1. `isLoading` - Boolean flag
2. `errorMessage` - Nullable string
3. `allPokemon` (or `livePokemon`) - Data list

## Prerequisites

Before starting Step 4, students should have completed:

- **Step 1**: Project setup, Tailwind CSS configuration
- **Step 2**: HttpClient setup, basic API calls, @foreach loops
- **Step 3**: LINQ Skip/Take pagination (helps understand list manipulation)

Understanding from Step 2 (HttpClient and GetFromJsonAsync) is critical for Step 4.

## Next Steps

After completing Step 4, students proceed to:

- **Step 5**: Search and filtering functionality
- **Step 6**: Detailed views and routing parameters

Step 4's loading/error patterns are used throughout Steps 5 and 6.

## Code Structure

### ExampleBase (Tutorial Page)

```csharp
public partial class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected HttpClient Http { get; set; } = default!;

    protected bool isLoading = true;
    protected string? errorMessage;
    protected List<PokemonItem> livePokemon = new();

    protected override async Task OnInitializedAsync()
    {
        // Check step completion + load live demo
    }

    private async Task LoadLiveDemoAsync()
    {
        try { ... }
        catch { ... }
        finally { isLoading = false; }
    }

    protected async Task RetryLoad()
    {
        await LoadLiveDemoAsync();
        StateHasChanged();
    }
}
```

### PokemonLoadingExampleBase (Demo Component)

```csharp
public partial class PokemonLoadingExampleBase : ComponentBase
{
    [Inject] protected HttpClient Http { get; set; } = default!;

    protected bool isLoading = true;
    protected string? errorMessage;
    protected List<PokemonItem> allPokemon = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadPokemonAsync();
    }

    private async Task LoadPokemonAsync()
    {
        try { ... }
        catch (HttpRequestException ex) { ... }
        catch (Exception ex) { ... }
        finally { isLoading = false; }
    }

    protected async Task RetryLoad()
    {
        await LoadPokemonAsync();
        StateHasChanged();
    }
}
```

## Styling Patterns

### Loading Spinner (Tailwind)

```html
<div
  class="animate-spin rounded-full h-12 w-12 border-4 
            border-blue-600 border-t-transparent"
></div>
```

Classes breakdown:

- `animate-spin` - Continuous rotation animation
- `rounded-full` - Makes it a perfect circle
- `h-12 w-12` - Size (12 × 0.25rem = 3rem = 48px)
- `border-4` - Thick border all around
- `border-blue-600` - Blue ring color
- `border-t-transparent` - Top border invisible (creates the "gap")

### Error Card

```html
<div class="bg-red-50 border-2 border-red-200 rounded-xl p-6">
  <h3 class="text-red-800 font-semibold mb-2">⚠️ Error</h3>
  <p class="text-red-700">Error message here</p>
  <button
    class="mt-4 px-4 py-2 bg-red-600 hover:bg-red-700 
                   text-white rounded-lg transition-colors"
  >
    Retry
  </button>
</div>
```

Red theme hierarchy:

- `bg-red-50` - Very light red background
- `border-red-200` - Light red border
- `text-red-700` - Medium-dark red text
- `text-red-800` - Darker red for headings
- `bg-red-600` - Bold red button

### Empty State

```html
<div class="text-center p-12 text-gray-500">
  <p class="text-lg font-medium">No Pokemon found</p>
  <p class="text-sm mt-2">Try again later or check your connection.</p>
</div>
```

Centered with gray text to indicate "nothing here" without alarming the user.

## Common Issues & Solutions

### Issue: Spinner shows forever after error

**Cause**: Forgot to add `finally` block, so `isLoading` never gets set to false when an exception is thrown.

**Solution**: Always use `finally` to ensure cleanup:

```csharp
try
{
    isLoading = true;
    data = await FetchAsync();
}
catch (Exception ex)
{
    errorMessage = ex.Message;
}
finally
{
    isLoading = false;  // CRITICAL: Always runs
}
```

### Issue: Error message doesn't show

**Cause**: Using wrong condition like `errorMessage != null` instead of checking for empty string.

**Solution**: Use `!string.IsNullOrEmpty(errorMessage)`:

```razor
@if (!string.IsNullOrEmpty(errorMessage))
{
    <!-- Error card -->
}
```

### Issue: Retry button doesn't work

**Cause**: Forgot to call `StateHasChanged()` after manually calling lifecycle method.

**Solution**: Always call StateHasChanged when manually re-running fetch logic:

```csharp
protected async Task RetryLoad()
{
    await LoadPokemonAsync();
    StateHasChanged();  // Tell Blazor to re-render
}
```

### Issue: UI doesn't update after retry

**Cause**: Same as above—StateHasChanged is missing.

**Why it's needed**: Blazor automatically re-renders after event handlers like `@onclick`, but when you manually call methods like `OnInitializedAsync`, Blazor doesn't know the state changed. You must explicitly trigger a re-render with `StateHasChanged()`.

### Issue: Different exception types not handled

**Cause**: Using single generic `catch (Exception ex)` block for everything.

**Solution**: Use multiple catch blocks for different error types:

```csharp
catch (HttpRequestException ex)
{
    errorMessage = "Network error. Please check your connection.";
}
catch (JsonException ex)
{
    errorMessage = "Invalid data format received.";
}
catch (Exception ex)
{
    errorMessage = "Something went wrong. Please try again.";
    Console.WriteLine($"Unexpected error: {ex}");
}
```

Order matters: Put more specific exceptions first, generic `Exception` last.

### Issue: Raw exception messages shown to users

**Cause**: Using `ex.Message` directly instead of user-friendly text.

**Solution**: Translate technical errors to friendly language:

```csharp
catch (HttpRequestException ex)
{
    // DON'T show: "The SSL connection could not be established..."
    // DO show:
    errorMessage = "Unable to connect to the server. Please check your internet connection.";

    // Log technical details for debugging:
    Console.WriteLine($"HttpRequestException: {ex}");
}
```

### Issue: Integer division in calculations

**Cause**: Dividing integers gives integer result, losing decimals.

**Solution**: Cast to double (though this is more relevant for Step 3 pagination, it's worth mentioning):

```csharp
// WRONG
int pages = count / pageSize;

// CORRECT
int pages = (int)Math.Ceiling(count / (double)pageSize);
```

## Related Resources

- [Microsoft Docs: Handle errors in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors)
- [Microsoft Docs: HttpClient factory](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests)
- [Tailwind CSS: Animation](https://tailwindcss.com/docs/animation)
- [PokeAPI Documentation](https://pokeapi.co/docs/v2)
- [C# try-catch-finally](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/exception-handling-statements)
- [Blazor Component Lifecycle](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle)
