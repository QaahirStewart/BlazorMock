using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step4;

public partial class PokemonLoadingExampleBase : ComponentBase
{
    // Dependency Injection
    [Inject] protected HttpClient Http { get; set; } = default!;

    // State
    protected bool isLoading = true;
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
            errorMessage = null;

            var response = await Http.GetFromJsonAsync<PokemonListResponse>(
                "https://pokeapi.co/api/v2/pokemon?limit=151"
            );

            allPokemon = response?.Results ?? new();
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
