using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step6;

public class PokemonSearchExampleBase : ComponentBase
{
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    protected bool isLoading = true;
    protected string errorMessage = string.Empty;
    protected List<PokemonItem> allPokemon = new();
    protected List<PokemonItem> CurrentPagePokemon = new();
    protected string searchQuery = "";
    
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages = 1;

    protected PokemonDetail? selectedPokemon = null;

    protected List<PokemonItem> FilteredPokemon
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

    protected override async Task OnInitializedAsync()
    {
        await LoadPokemonAsync();
    }

    private async Task LoadPokemonAsync()
    {
        isLoading = true;
        errorMessage = string.Empty;

        try
        {
            var http = HttpClientFactory.CreateClient("PokeApi");
            var response = await http.GetFromJsonAsync<PokemonListResponse>(
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

    protected void OnSearchChanged()
    {
        currentPage = 1;
        UpdatePagination();
    }

    protected void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdatePagination();
        }
    }

    protected void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            UpdatePagination();
        }
    }

    private void UpdatePagination()
    {
        var filtered = FilteredPokemon;
        totalPages = (int)Math.Ceiling(filtered.Count / (double)pageSize);
        
        if (currentPage > totalPages && totalPages > 0)
            currentPage = totalPages;

        CurrentPagePokemon = filtered
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    protected async Task ShowDetails(int id)
    {
        try
        {
            var http = HttpClientFactory.CreateClient("PokeApi");
            selectedPokemon = await http.GetFromJsonAsync<PokemonDetail>(
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

    protected int GetPokemonId(string url)
    {
        var parts = url.TrimEnd('/').Split('/');
        return int.Parse(parts[^1]);
    }

    protected string GetSpriteUrl(int id)
    {
        return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
    }

    public class PokemonListResponse
    {
        [JsonPropertyName("results")]
        public List<PokemonItem> Results { get; set; } = new();
    }

    public class PokemonItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("url")]
        public string Url { get; set; } = "";
    }

    public class PokemonDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("base_experience")]
        public int Base_Experience { get; set; }

        [JsonPropertyName("types")]
        public List<PokemonType> Types { get; set; } = new();

        [JsonPropertyName("abilities")]
        public List<PokemonAbility> Abilities { get; set; } = new();
    }

    public class PokemonType
    {
        [JsonPropertyName("type")]
        public TypeInfo Type { get; set; } = new();
    }

    public class TypeInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class PokemonAbility
    {
        [JsonPropertyName("ability")]
        public AbilityInfo Ability { get; set; } = new();
    }

    public class AbilityInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }
}
