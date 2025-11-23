using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step5;

public partial class PokemonCardsExampleBase : ComponentBase
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;

    protected bool isLoading = true;
    protected string? errorMessage;
    protected List<PokemonItem> allPokemon = new();
    protected List<PokemonItem> currentPagePokemon = new();
    
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages = 1;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            isLoading = true;
            errorMessage = null;

            var client = HttpClientFactory.CreateClient("PokeApi");
            var response = await client.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=151");

            allPokemon = response?.Results ?? new List<PokemonItem>();
            totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);
            UpdateCurrentPage();
        }
        catch (HttpRequestException ex)
        {
            errorMessage = $"Network error: {ex.Message}";
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

    protected void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdateCurrentPage();
        }
    }

    protected void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            UpdateCurrentPage();
        }
    }

    private void UpdateCurrentPage()
    {
        currentPagePokemon = allPokemon
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
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

    protected class PokemonListResponse
    {
        public List<PokemonItem> Results { get; set; } = new();
    }

    protected class PokemonItem
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
