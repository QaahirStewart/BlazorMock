# Pokemon Step 6: Search, Filter & Details

## Overview

Step 6 completes the Pokemon explorer by adding search functionality, detailed Pokemon information modal, and professional interaction patterns. This final step combines all previous concepts (API calls, pagination, state management, responsive design) with search filtering and modal dialogs.

**Key Learning**: Search and detail views are essential features in most applications. This step teaches real-time filtering, case-insensitive matching, modal overlays, and proper event handling patterns that students will use in every app they build.

## Files in This Folder

- **`Example.razor`**: Tutorial page with comprehensive explanation of search, filtering, modals, and event modifiers with live demo
- **`Example.razor.cs`**: Code-behind with search logic, pagination, API calls for details, and DTOs
- **`README.md`**: This file - documentation for maintainers and instructors

## Routes

- **Tutorial**: `/pokemon-examples/step6`
- **Live Demo**: Integrated into tutorial page (searchable Pokemon grid with modal details)

## What Students Learn

### Core Concepts

1. **Real-Time Search Filtering**

   - @bind:event="oninput" for updates on every keystroke
   - @bind:after to trigger actions after binding updates
   - Filtering collections with LINQ Where()
   - Case-insensitive string matching with StringComparison.OrdinalIgnoreCase

2. **Pagination with Search**

   - Resetting currentPage to 1 when search changes
   - Recalculating totalPages based on filtered results
   - Handling edge cases (empty results, page > totalPages)

3. **API Detail Endpoints**

   - Fetching individual Pokemon details from /pokemon/{id}
   - Deserializing nested JSON structures (types, abilities)
   - Converting API units (decimeters → meters, hectograms → kilograms)

4. **Modal Overlay Pattern**

   - Fixed positioning with full-screen overlay
   - Semi-transparent backgrounds (bg-black/50)
   - Click outside to close with @onclick
   - @onclick:stopPropagation to prevent content clicks from closing

5. **Event Handling**
   - Event bubbling and how it affects modals
   - stopPropagation to prevent event bubbling
   - Lambda expressions in event handlers: @onclick="@(() => ShowDetails(id))"

### Technical Skills

- Implementing real-time search with @bind:event and @bind:after
- Filtering collections with case-insensitive string matching
- Managing pagination state with dynamic filtered data
- Fetching and deserializing complex API responses
- Creating modal dialogs with proper UX (click outside to close)
- Preventing event bubbling with @onclick:stopPropagation
- Displaying nested data structures (types, abilities)

## Key Concepts Deep Dive

### Search Pattern with @bind:after

```razor
<input type="text"
       @bind="searchQuery"
       @bind:event="oninput"
       @bind:after="OnSearchChanged"
       placeholder="Search..." />

@code {
    string searchQuery = "";
    int currentPage = 1;

    void OnSearchChanged()
    {
        currentPage = 1;  // Reset to first page
        UpdatePagination();
    }
}
```

**Why this matters**:

- @bind:event="oninput" updates on every keystroke (real-time)
- @bind:after="OnSearchChanged" runs automatically after binding updates
- OnSearchChanged() resets pagination so users see page 1 of filtered results

### Case-Insensitive Filtering

```csharp
List<Pokemon> FilteredPokemon => string.IsNullOrWhiteSpace(searchQuery)
    ? allPokemon
    : allPokemon
        .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
        .ToList();
```

**StringComparison.OrdinalIgnoreCase**:

- "pikachu" matches "Pikachu", "PIKACHU", "PiKaChU"
- Without it, search is case-sensitive (frustrating for users)
- Fast and culture-independent (perfect for simple search)

### Modal Overlay Pattern

```razor
@if (selectedPokemon != null)
{
    <!-- Overlay: Click to close -->
    <div @onclick="CloseModal"
         class="fixed inset-0 bg-black/50 flex items-center justify-center">

        <!-- Content: stopPropagation prevents close -->
        <div @onclick:stopPropagation
             class="bg-white rounded-2xl p-6 max-w-md">

            <h2>@selectedPokemon.Name</h2>
            <button @onclick="CloseModal">Close</button>
        </div>
    </div>
}
```

**Event flow**:

1. User clicks overlay → CloseModal() runs → Modal closes ✓
2. User clicks content → stopPropagation prevents bubbling → Modal stays open ✓
3. User clicks Close button → CloseModal() runs explicitly → Modal closes ✓

## Architecture

### Component Structure

```
Pokemon/Step6/
├── Example.razor          # Tutorial page with search demo
├── Example.razor.cs       # Code-behind with search logic and DTOs
└── README.md             # This documentation
```

### Code-Behind Pattern

**ExampleBase** inherits from `ComponentBase` and includes:

- **Injected Services**: ILearningProgressService, HttpClient, NavigationManager, IJSRuntime
- **State Management**: searchQuery, allPokemon, FilteredPokemon, CurrentPagePokemon, selectedPokemon
- **Search Logic**: OnSearchChanged(), FilteredPokemon computed property
- **Pagination Logic**: PreviousPage(), NextPage(), UpdatePagination()
- **Modal Logic**: ShowDetails(id), CloseModal()
- **Helper Methods**: GetPokemonId(url), GetSpriteUrl(id)
- **DTOs**: PokemonDetail, PokemonType, PokemonAbility, TypeInfo, AbilityInfo

### Data Flow

1. **Initial Load**: LoadPokemonAsync() → allPokemon (151 Pokemon)
2. **User Types**: searchQuery updates → FilteredPokemon recalculates → OnSearchChanged() resets page
3. **Pagination**: CurrentPagePokemon = FilteredPokemon.Skip().Take()
4. **Card Click**: ShowDetails(id) → Fetch from API → selectedPokemon = result
5. **Modal Displays**: @if (selectedPokemon != null) renders overlay
6. **Close Modal**: CloseModal() → selectedPokemon = null

## Prerequisites

Students should have completed:

- **Step 1**: Project setup, Tailwind configuration
- **Step 2**: Basic API calls and data display
- **Step 3**: Client-side pagination
- **Step 4**: Loading and error state handling
- **Step 5**: Responsive design and visual polish

Step 6 combines all previous concepts with search and modal interactions.

## Next Steps

Step 6 is the final step in the Pokemon tutorial series. Students have now built a complete, production-ready Pokemon explorer with:

- Search and filter functionality
- Pagination
- Loading and error states
- Responsive design
- Modal detail views
- Professional polish

Students can extend this project with:

- Type filtering (dropdown to filter by type)
- Favorites system (localStorage to save favorites)
- Sorting (by name, ID, height, weight)
- Advanced search (search by type, ability, stats)

## Code Structure

### State Variables

```csharp
protected string searchQuery = "";              // User's search input
protected List<PokemonListItem> allPokemon;     // All 151 Pokemon from API
protected int currentPage = 1;                  // Current page number
protected int pageSize = 20;                    // Items per page
protected int totalPages = 0;                   // Total pages (recalculated on search)
protected PokemonDetail? selectedPokemon;       // Currently selected Pokemon for modal
```

### Computed Properties

```csharp
// Returns filtered Pokemon based on search query
protected List<PokemonListItem> FilteredPokemon { get; }

// Returns current page of filtered Pokemon
protected List<PokemonListItem> CurrentPagePokemon { get; }
```

### API Endpoints

- **List**: `https://pokeapi.co/api/v2/pokemon?limit=151`
- **Details**: `https://pokeapi.co/api/v2/pokemon/{id}`

**Detail Response**:

- Height in decimeters (7 decimeters = 0.7 meters)
- Weight in hectograms (69 hectograms = 6.9 kilograms)
- Types: Array of `{ type: { name: "fire" } }`
- Abilities: Array of `{ ability: { name: "blaze" } }`

## Common Issues & Solutions

### Search Doesn't Work

**Problem**: Search doesn't find any results even when typing exact names.

**Solutions**:

- Use `StringComparison.OrdinalIgnoreCase` in Contains()
- Check that searchQuery is trimmed properly
- Verify LINQ Where() is called on allPokemon, not filtered results
- Ensure @bind:event="oninput" is present for real-time updates

### Stuck on Wrong Page After Search

**Problem**: After searching, user sees "Page 3 of 1" or empty results.

**Solutions**:

- Add `@bind:after="OnSearchChanged"` to reset currentPage
- In OnSearchChanged(), set `currentPage = 1`
- Recalculate totalPages based on filtered count
- Ensure UpdatePagination() is called after search changes

### Modal Closes When Clicking Content

**Problem**: Clicking anywhere in modal closes it, even when clicking Pokemon details.

**Solutions**:

- Add `@onclick:stopPropagation` to modal content div
- Overlay div should have `@onclick="CloseModal"`
- Content div should have `@onclick:stopPropagation`
- Verify stopPropagation is on the correct element (content, not overlay)

### Can't Close Modal

**Problem**: Modal opens but won't close when clicking outside or Close button.

**Solutions**:

- Ensure CloseModal() sets `selectedPokemon = null`
- Modal should only render when `@if (selectedPokemon != null)`
- Overlay needs `@onclick="CloseModal"`
- Close button needs `@onclick="CloseModal"`

### Types/Abilities Don't Display

**Problem**: Pokemon modal shows but types and abilities are missing.

**Solutions**:

- Check nested structure: `type.Type.Name` (capital T)
- Abilities: `ability.Ability.Name` (capital A)
- Ensure DTO classes match API response exactly
- Verify API returns data (some Pokemon have no abilities)

### Height/Weight Show Wrong Values

**Problem**: Pikachu shows height 4 instead of 0.4 meters.

**Solutions**:

- Divide height by 10.0: `selectedPokemon.Height / 10.0`
- Divide weight by 10.0: `selectedPokemon.Weight / 10.0`
- Use decimal division (10.0, not 10) to get decimal result
- Display with unit: `@(height / 10.0)m` not just `@(height / 10.0)`

### Search Updates on Blur, Not Typing

**Problem**: Search only updates when user clicks away from input.

**Solutions**:

- Add `@bind:event="oninput"` to input element
- Default behavior is `onchange` (updates on blur)
- oninput updates on every keystroke
- Verify both @bind and @bind:event are present

## Related Tips

Students should review these tips from the Tips page:

- **@bind:after** (#bindafter): Run code automatically after binding updates
- **@bind:event** (#bindevent): Choose when binding updates (oninput vs onchange)
- **@onclick:stopPropagation** (#onclickstoppropagation): Prevent event bubbling in modals
- **String.Contains (Case-Insensitive)** (#stringcontains-case-insensitive): Flexible search matching
- **Modal Overlay Pattern** (#modal-overlay-pattern): Professional modal dialogs

## Teaching Notes

### Key Points to Emphasize

1. **Always reset pagination on search**: Users expect page 1 of filtered results
2. **Use OrdinalIgnoreCase**: Makes search flexible and user-friendly
3. **stopPropagation is essential for modals**: Prevents unwanted closes
4. **oninput for search, onchange for forms**: Different use cases
5. **Divide API units**: PokeAPI uses decimeters and hectograms

### Common Student Mistakes

- Forgetting @bind:after to reset pagination
- Using case-sensitive Contains() without OrdinalIgnoreCase
- Putting stopPropagation on overlay instead of content
- Not dividing height/weight by 10
- Using async void instead of async Task for event handlers

### Demo Suggestions

- Search for "char" to show Charizard, Charmander, Charmeleon
- Search for "PIKA" to demonstrate case-insensitive matching
- Show what happens without pagination reset (page 5 of 1 results)
- Demonstrate modal: click outside to close vs click content stays open
- Show height/weight conversion (Pikachu: 4 decimeters → 0.4 meters)

## Maintenance Notes

- Uses PokeAPI for data (https://pokeapi.co)
- Sprites from official GitHub repo (PokeAPI/sprites)
- Search logic assumes Pokemon names are in English
- Modal z-index (z-50) must be higher than other elements
- Height and weight units require division by 10
- Types and abilities are nested structures in API response
