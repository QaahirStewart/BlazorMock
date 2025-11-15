# Pokemon Step 3: Add Paging

## Overview

This step teaches client-side pagination to browse through a large Pokemon list in manageable chunks without making additional API calls.

## Files in This Folder

- **Example.razor** - Tutorial page markup teaching pagination concepts
- **Example.razor.cs** - Code-behind with progress tracking and live demo pagination logic
- **PokemonPagedListExample.razor** - Demo component markup showing pagination UI
- **PokemonPagedListExample.razor.cs** - Code-behind with paging state and helper methods
- **README.md** - This documentation file

## Routes

- Tutorial page: `/pokemon-examples/step3`
- Demo component: `/pokemon-examples/demo-paged`

## What Students Learn

1. Calculate total pages using `Math.Ceiling` to handle partial pages
2. Use LINQ `Skip()` and `Take()` methods to slice lists into pages
3. Implement Previous/Next navigation with boundary checking
4. Disable buttons at list boundaries (first/last page)
5. Track current page state and update UI reactively
6. Store all data in memory for instant client-side paging

## Key Concepts

- **Client-side paging**: Fetch all data once, slice in the browser
- **Skip/Take pattern**: LINQ methods for paginating collections
- **Math.Ceiling**: Round up to ensure partial last page is counted
- **Button disabled state**: Prevent navigation beyond valid page range
- **Reactive state**: Component re-renders when `currentPage` changes

## Architecture

### Code-Behind Pattern

- **Example.razor** - Pure markup, inherits from `ExampleBase`
- **Example.razor.cs** - `ExampleBase` class with live demo pagination logic
- **PokemonPagedListExample.razor** - Pure markup, inherits from `PokemonPagedListExampleBase`
- **PokemonPagedListExample.razor.cs** - `PokemonPagedListExampleBase` with paging implementation

### Dependency Injection

- `ILearningProgressService` - Track step completion (tutorial page)
- `NavigationManager` - Handle navigation (tutorial page)
- `IJSRuntime` - Code block enhancement (tutorial page)
- `IHttpClientFactory` - Create configured HttpClient for PokeAPI (demo component)

### Inheritance

Both components use `@inherits` to separate markup from logic:

```razor
@inherits ExampleBase               // Tutorial page
@inherits PokemonPagedListExampleBase  // Demo component
```

## Prerequisites

- **Step 2 completed**: Understanding of HTTP calls and list rendering
- **HttpClient configured**: Named "PokeApi" client registered in Program.cs
- **PokeAPI accessible**: https://pokeapi.co/api/v2/

## Next Steps

- **Step 4**: Add search/filter functionality
- Reset to page 1 when applying filters
- Consider adding direct page number navigation
- Explore server-side paging for larger datasets

## Code Structure

### Paging State (Demo Component)

```csharp
protected List<PokemonItem>? allPokemon;  // Full list stored in memory
protected int currentPage = 1;             // Current page (1-based)
protected int pageSize = 20;               // Items per page
protected int totalPages = 1;              // Calculated: Ceiling(count / pageSize)
```

### Skip/Take Formula

```csharp
allPokemon
    .Skip((currentPage - 1) * pageSize)  // Skip previous pages
    .Take(pageSize)                       // Take current page items
    .ToList()
```

**Examples**:

- Page 1: Skip(0), Take(20) → items 1-20
- Page 2: Skip(20), Take(20) → items 21-40
- Page 8: Skip(140), Take(20) → items 141-151 (partial page)

### Total Pages Calculation

```csharp
totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);
```

**Why Math.Ceiling?**

- 151 items ÷ 20 per page = 7.55 pages
- `Math.Ceiling(7.55)` = 8 pages
- Without Ceiling, last 11 Pokemon wouldn't have a page

### Navigation Guards

```csharp
protected void NextPage()
{
    if (currentPage < totalPages)  // Prevent going past last page
    {
        currentPage++;
    }
}

protected void PreviousPage()
{
    if (currentPage > 1)  // Prevent going before first page
    {
        currentPage--;
    }
}
```

## Styling Patterns

- **Navigation container**: `flex items-center justify-between mb-4`
- **Buttons**: `px-3 py-1 rounded-lg border text-sm`
- **Disabled state**: `disabled:opacity-50 disabled:cursor-not-allowed`
- **Page indicator**: `text-sm text-gray-600` with `font-semibold` for numbers
- **Grid layout**: `grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-3`
- **Pokemon cards**: `bg-white border border-gray-200 rounded-xl px-3 py-2`

## Common Issues & Solutions

### Buttons Don't Disable

**Issue**: Previous/Next buttons always enabled  
**Cause**: Incorrect `disabled` attribute syntax  
**Solution**: Use `disabled="@(condition)"` not `disabled="condition"`

```razor
<!-- WRONG -->
<button disabled="currentPage == 1">

<!-- CORRECT -->
<button disabled="@(currentPage == 1)">
```

### Empty Pages at End

**Issue**: Last page shows no items  
**Cause**: Integer division truncates decimal in totalPages calculation  
**Solution**: Cast to `(double)pageSize` before dividing

```csharp
// WRONG
totalPages = (int)(allPokemon.Count / pageSize);  // 151 / 20 = 7

// CORRECT
totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);  // 8
```

### Wrong Items Showing

**Issue**: Skip/Take shows incorrect items for current page  
**Cause**: Incorrect Skip formula  
**Solution**: Use `(currentPage - 1) * pageSize`, not `currentPage * pageSize`

```csharp
// WRONG
.Skip(currentPage * pageSize)  // Page 1 skips 20 items!

// CORRECT
.Skip((currentPage - 1) * pageSize)  // Page 1 skips 0 items
```

### Page Count is Zero

**Issue**: `totalPages` remains 0 or 1  
**Cause**: Calculation happens before `allPokemon` is populated  
**Solution**: Calculate totalPages AFTER data is fetched in `OnInitializedAsync`

```csharp
protected override async Task OnInitializedAsync()
{
    var response = await client.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=151");
    allPokemon = response?.Results ?? new List<PokemonItem>();

    // Calculate AFTER allPokemon is populated
    totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);
}
```

### Orphaned Files After Move

**Issue**: Build error "The type or namespace name 'ExampleBase' could not be found"  
**Cause**: Old `PokemonStep3Example.razor` file still exists at original location  
**Solution**: Delete orphaned file

```powershell
Remove-Item -Path "d:\BlazorMock\Components\Pages\Examples\PokemonStep3Example.razor" -Force
```

## Related Resources

- [LINQ Skip Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skip)
- [LINQ Take Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.take)
- [Math.Ceiling Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.math.ceiling)
- [Blazor Component State](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/state-management)
- [Pagination Best Practices](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page)
