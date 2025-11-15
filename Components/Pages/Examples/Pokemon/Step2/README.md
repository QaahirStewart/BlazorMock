# Pokemon Step 2: Call PokeAPI and Render List

## Overview

This step teaches how to call the PokeAPI from a Blazor component, deserialize JSON into C# models, and render a list of Pokemon.

## Files in This Folder

### Tutorial Files

- **`Example.razor`** - Main tutorial page (markup only)
- **`Example.razor.cs`** - Code-behind for tutorial page with:
  - Progress tracking (`isComplete`, `MarkComplete`, `ResetStep`)
  - Live demo data loading
  - JavaScript interop for code block enhancement
  - Helper methods for sprite URL building

### Demo Component Files

- **`PokemonListExample.razor`** - Demo component students create (markup only)
- **`PokemonListExample.razor.cs`** - Code-behind for demo component with:
  - HTTP client injection
  - API call logic in `OnInitializedAsync`
  - DTO models (`PokemonListResponse`, `PokemonItem`)

### This Documentation

- **`README.md`** - This file explaining the folder structure and purpose

## Routes

- **Tutorial Page**: `/pokemon-examples/step2`
- **Demo Component**: `/pokemon-examples/demo-list`

## What Students Learn

1. Using `IHttpClientFactory` to create named HTTP clients
2. Calling `GetFromJsonAsync<T>` to fetch and deserialize JSON
3. Creating C# DTO models that match API JSON structure
4. Using `@foreach` in Razor to render lists
5. Handling loading states with null checks

## Key Concepts

- **HTTP Clients**: Named clients configured in `Program.cs`
- **JSON Deserialization**: Built-in .NET JSON serialization
- **DTO Models**: Data Transfer Objects for API responses
- **Razor Loops**: Using `@foreach` for dynamic rendering

## Architecture

### Code-Behind Pattern

Both the tutorial page and demo component use code-behind files to separate concerns:

- `.razor` files contain only markup and Razor syntax
- `.razor.cs` files contain all C# logic, state, and dependencies

### Dependency Injection

- Tutorial page injects: `ILearningProgressService`, `NavigationManager`, `IJSRuntime`
- Demo component injects: `IHttpClientFactory`

### Component Inheritance

- Tutorial page: `@inherits ExampleBase`
- Demo component: `@inherits PokemonListExampleBase`

## Live Demo Data

The tutorial page includes a working live demo that:

- Fetches 8 Pokemon from the PokeAPI
- Parses Pokemon IDs from URLs
- Builds sprite image URLs
- Renders cards with Pokemon images and names

## Prerequisites

Students should have completed:

- Step 0: Configure HttpClient for PokeAPI
- Step 1: Create Blazor project and set up Tailwind CSS

## Next Steps

After completing this step, students move to:

- Step 3: Add pagination to the Pokemon list

## Code Structure

### Tutorial Page (Example.razor.cs)

```csharp
public partial class ExampleBase : ComponentBase, IDisposable
{
    // Services
    [Inject] protected ILearningProgressService ProgressService { get; set; }
    [Inject] protected NavigationManager Navigation { get; set; }
    [Inject] protected IJSRuntime JS { get; set; }

    // State
    protected bool isComplete;
    protected List<LivePokemonItem>? livePokemon;

    // Lifecycle
    protected override async Task OnInitializedAsync() { }
    protected override async Task OnAfterRenderAsync(bool firstRender) { }

    // Handlers
    protected async Task MarkComplete() { }
    protected async Task ResetStep() { }

    // Helpers
    protected static string? BuildSpriteUrlFromUrl(string? apiUrl) { }
}
```

### Demo Component (PokemonListExample.razor.cs)

```csharp
public partial class PokemonListExampleBase : ComponentBase
{
    // Services
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; }

    // State
    protected List<PokemonItem>? pokemon;

    // Lifecycle
    protected override async Task OnInitializedAsync()
    {
        var client = HttpClientFactory.CreateClient("PokeApi");
        var response = await client.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=20");
        pokemon = response?.Results ?? new List<PokemonItem>();
    }

    // Models
    protected class PokemonListResponse { }
    protected class PokemonItem { }
}
```

## Styling Patterns

- **Gradient Banners**: `bg-gradient-to-r from-gray-100 to-blue-100` for "What You'll Learn" and "How to do it"
- **White Cards**: `bg-white rounded-xl p-6 border border-gray-200` for main content
- **Pro Tips**: `bg-blue-50 border border-blue-200` for helpful tips
- **Troubleshooting**: `bg-yellow-50 border border-yellow-200` for common issues

## Common Issues & Solutions

1. **Null responses**: Verify `PokeApi` HttpClient is registered in `Program.cs`
2. **Network errors**: Check browser console for failed API calls
3. **Property name mismatches**: Ensure DTO property names match JSON (case-sensitive)

## Related Resources

- PokeAPI Documentation: https://pokeapi.co/docs/v2
- .NET HttpClient: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
- JSON Serialization: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json
