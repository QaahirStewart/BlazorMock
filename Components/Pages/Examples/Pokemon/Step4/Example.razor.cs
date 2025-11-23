using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;
using System.Net.Http.Json;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step4;

public partial class ExampleBase : ComponentBase, IDisposable
{
    // Dependency Injection
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;
    [Inject] protected HttpClient Http { get; set; } = default!;

    // State
    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
    
    // Live demo state
    protected bool isLoading = true;
    protected bool showImages = false;
    protected string? errorMessage;
    protected List<PokemonItem> livePokemon = new();

    // Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 4);
        isComplete = step?.IsComplete ?? false;

        // Load live demo data
        await LoadLiveDemoAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _copyModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/codeblocks.js");
                await _copyModule.InvokeVoidAsync("enhancePreBlocks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load code block enhancer: {ex.Message}");
            }
        }
    }

    // Event Handlers
    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("pokemon", 4);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 4);
        isComplete = false;
    }

    protected async Task RetryLoad()
    {
        await LoadLiveDemoAsync();
        StateHasChanged();
    }

    protected async Task RefreshDemo()
    {
        await LoadLiveDemoAsync();
        StateHasChanged();
    }

    // Helper Methods
    private async Task LoadLiveDemoAsync()
    {
        try
        {
            isLoading = true;
            showImages = false;
            errorMessage = null;

            // Initial delay to show skeleton loading
            await Task.Delay(500);

            var response = await Http.GetFromJsonAsync<PokemonListResponse>(
                "https://pokeapi.co/api/v2/pokemon?limit=20"
            );

            livePokemon = response?.Results ?? new();
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

    // Cleanup
    public void Dispose()
    {
        _copyModule?.DisposeAsync();
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
