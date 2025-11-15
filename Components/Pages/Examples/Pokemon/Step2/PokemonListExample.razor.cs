using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step2;

public partial class PokemonListExampleBase : ComponentBase
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;

    protected List<PokemonItem>? pokemon;

    protected override async Task OnInitializedAsync()
    {
        var client = HttpClientFactory.CreateClient("PokeApi");
        var response = await client.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=20");

        pokemon = response?.Results ?? new List<PokemonItem>();
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
