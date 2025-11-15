using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step3;

public partial class PokemonPagedListExampleBase : ComponentBase
{
    // Dependency Injection
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;

    // State
    protected List<PokemonItem>? allPokemon;
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages = 1;

    // Lifecycle
    protected override async Task OnInitializedAsync()
    {
        var client = HttpClientFactory.CreateClient("PokeApi");
        var response = await client.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=151");

        allPokemon = response?.Results ?? new List<PokemonItem>();
        totalPages = Math.Max(1, (int)Math.Ceiling(allPokemon.Count / (double)pageSize));
    }

    // Helper methods
    protected List<PokemonItem> GetPagedPokemon()
    {
        if (allPokemon is null || allPokemon.Count == 0)
        {
            return new List<PokemonItem>();
        }

        return allPokemon
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    protected void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
        }
    }

    protected void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
        }
    }

    // DTOs
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
