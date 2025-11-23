using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step4;

public partial class PokemonLoadingExampleBase : ComponentBase
{
    // Dependency Injection
    [Inject] protected HttpClient Http { get; set; } = default!;

    // State
    protected bool isLoading = true;
    protected bool showImages = false;
    protected string? errorMessage;
    protected List<PokemonItem> allPokemon = new();

    // Lifecycle
    protected override async Task OnInitializedAsync()
    {
        await LoadPokemonAsync();
    }

    // Helper methods
    protected async Task RetryLoad()
    {
        await LoadPokemonAsync();
        StateHasChanged();
    }

    private async Task LoadPokemonAsync()
    {
        try
        {
            isLoading = true;
            showImages = false;
            errorMessage = null;

            // Initial delay to show skeleton loading
            await Task.Delay(500);

            var response = await Http.GetFromJsonAsync<PokemonListResponse>(
                "https://pokeapi.co/api/v2/pokemon?limit=151"
            );

            allPokemon = response?.Results ?? new();
            isLoading = false; // Names render immediately

            // Extra delay before showing images
            await Task.Delay(500);
            showImages = true;
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

    // Helper methods
    protected string GetPokemonId(string url)
    {
        var parts = url.TrimEnd('/').Split('/');
        return parts[^1];
    }

    protected string GetPokemonImageUrl(string url)
    {
        var id = GetPokemonId(url);
        return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
    }

    // DTOs
    protected sealed class PokemonListResponse
    {
        public List<PokemonItem> Results { get; set; } = new();
    }

    protected sealed class PokemonItem
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
