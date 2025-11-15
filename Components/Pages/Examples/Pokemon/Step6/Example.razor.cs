using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;
using System.Net.Http.Json;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step6;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;
    [Inject] protected HttpClient Http { get; set; } = default!;

    protected bool isComplete;
    protected bool isLoading = false;
    protected string errorMessage = string.Empty;
    protected string searchQuery = "";
    
    protected List<PokemonListItem> allPokemon = new();
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages = 0;
    
    protected PokemonDetail? selectedPokemon;

    private bool _disposed = false;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 6);
        isComplete = step?.IsComplete ?? false;

        await LoadPokemonAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await JS.InvokeVoidAsync("window.setupCodeBlocks");
            }
            catch
            {
                // JS might not be loaded yet, that's ok
            }
        }
    }

    private async Task LoadPokemonAsync()
    {
        isLoading = true;
        errorMessage = string.Empty;

        try
        {
            var response = await Http.GetFromJsonAsync<PokemonListResponse>(
                "https://pokeapi.co/api/v2/pokemon?limit=151");

            if (response?.Results != null)
            {
                allPokemon = response.Results;
                UpdatePagination();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load Pokemon: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    protected List<PokemonListItem> FilteredPokemon
    {
        get
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return allPokemon;
            
            return allPokemon
                .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    protected List<PokemonListItem> CurrentPagePokemon
    {
        get
        {
            var filtered = FilteredPokemon;
            return filtered
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }

    protected void OnSearchChanged()
    {
        currentPage = 1;
        UpdatePagination();
    }

    protected void UpdatePagination()
    {
        var filteredCount = FilteredPokemon.Count;
        totalPages = filteredCount == 0 ? 1 : (int)Math.Ceiling(filteredCount / (double)pageSize);
        
        // Ensure current page is valid
        if (currentPage > totalPages)
            currentPage = totalPages;
    }

    protected void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
        }
    }

    protected void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
        }
    }

    protected async Task ShowDetails(int id)
    {
        try
        {
            selectedPokemon = await Http.GetFromJsonAsync<PokemonDetail>(
                $"https://pokeapi.co/api/v2/pokemon/{id}");
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load Pokemon details: {ex.Message}";
        }
    }

    protected void CloseModal()
    {
        selectedPokemon = null;
    }

    protected static int GetPokemonId(string url)
    {
        var parts = url.TrimEnd('/').Split('/');
        return int.Parse(parts[^1]);
    }

    protected static string GetSpriteUrl(int id)
    {
        return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
    }

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("pokemon", 6);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 6);
        isComplete = false;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }

    // DTOs for PokeAPI response
    public class PokemonListResponse
    {
        public List<PokemonListItem> Results { get; set; } = new();
    }

    public class PokemonListItem
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class PokemonDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }  // In decimeters
        public int Weight { get; set; }  // In hectograms
        public int Base_Experience { get; set; }
        public List<PokemonType> Types { get; set; } = new();
        public List<PokemonAbility> Abilities { get; set; } = new();
    }

    public class PokemonType
    {
        public TypeInfo Type { get; set; } = new();
    }

    public class TypeInfo
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokemonAbility
    {
        public AbilityInfo Ability { get; set; } = new();
    }

    public class AbilityInfo
    {
        public string Name { get; set; } = string.Empty;
    }
}
