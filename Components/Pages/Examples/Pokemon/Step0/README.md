# Pokemon Step 0: Configure HttpClient for PokeAPI

## Overview

This foundational step teaches how to configure a named HttpClient in Blazor for making API calls to the PokeAPI. This setup is essential for all subsequent Pokemon tutorial steps.

## Files in This Folder

### Tutorial Files

- **`Example.razor`** - Main tutorial page (markup only)
- **`Example.razor.cs`** - Code-behind for tutorial page with:
  - Progress tracking (`isComplete`, `MarkComplete`, `ResetStep`)
  - JavaScript interop for code block enhancement
  - No live demo for this configuration step

### This Documentation

- **`README.md`** - This file explaining the folder structure and purpose

## Routes

- **Tutorial Page**: `/pokemon-examples/step0`

## What Students Learn

1. How to register a named `HttpClient` using `AddHttpClient`
2. Configuring a base address for API endpoints
3. Using dependency injection to access `IHttpClientFactory`
4. Creating named client instances in components
5. Understanding the benefits of named clients over creating `HttpClient` instances directly

## Key Concepts

- **Named HttpClients**: Reusable, configured HTTP clients registered via DI
- **Base Addresses**: Pre-configured URL bases to simplify relative path requests
- **Dependency Injection**: ASP.NET Core's service container pattern
- **`IHttpClientFactory`**: Factory pattern for creating HttpClient instances

## Architecture

### Code-Behind Pattern

The tutorial page uses code-behind to separate concerns:

- `.razor` file contains only markup and Razor syntax
- `.razor.cs` file contains all C# logic, state, and dependencies

### Dependency Injection

Tutorial page injects:

- `ILearningProgressService` - For tracking step completion
- `NavigationManager` - For navigation (available but not actively used)
- `IJSRuntime` - For JavaScript interop (code block enhancement)

### Component Inheritance

- Tutorial page: `@inherits ExampleBase`

## No Live Demo

This step focuses on configuration rather than visible functionality, so there is no live demo component. Students will see the results of this configuration in Step 2 when they make actual API calls.

## Prerequisites

Students should have:

- A Blazor Server project created
- Basic understanding of dependency injection
- Access to `Program.cs` for service registration

## Next Steps

After completing this step, students move to:

- Step 1: Set up Tailwind CSS (optional styling step)
- Step 2: Call PokeAPI and render a list of Pokemon (uses the configured HttpClient)

## Configuration Example

### In Program.cs

```csharp
builder.Services.AddHttpClient("PokeApi", client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
});
```

### In a Razor Component

```razor
@inject IHttpClientFactory HttpClientFactory

@code {
    protected override async Task OnInitializedAsync()
    {
        var client = HttpClientFactory.CreateClient("PokeApi");
        // Now you can use this client to make API calls
    }
}
```

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

    // Lifecycle
    protected override async Task OnInitializedAsync()
    {
        // Load completion status
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Enhance code blocks with copy functionality
    }

    // Handlers
    protected async Task MarkComplete() { }
    protected async Task ResetStep() { }
}
```

## Styling Patterns

- **Gradient Banners**: `bg-gradient-to-r from-gray-100 to-blue-100` for "What you'll do" and "How to do it"
- **White Cards**: `bg-white rounded-xl p-6 border border-gray-200` for main content
- **Code Examples**: `bg-gray-100 border border-gray-200 rounded-lg p-4` for code snippets
- **Completion Card**: `bg-white border-2 border-green-200` for the mark complete section

## Why Named HttpClients?

1. **Reusability**: Configure once, use everywhere
2. **Proper Disposal**: `IHttpClientFactory` handles lifecycle management
3. **Testability**: Easy to mock in unit tests
4. **Configuration**: Centralized setup for base URLs, headers, timeouts, etc.
5. **Best Practice**: Avoids socket exhaustion from improper `HttpClient` disposal

## Common Issues & Solutions

1. **Client not found**: Ensure the name matches exactly between registration and `CreateClient()`
2. **Base URL not working**: Verify the URL ends without a trailing slash in some scenarios
3. **Service not available**: Make sure `AddHttpClient` is called before `builder.Build()`

## Related Resources

- IHttpClientFactory Documentation: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
- PokeAPI Documentation: https://pokeapi.co/docs/v2
- Dependency Injection in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
