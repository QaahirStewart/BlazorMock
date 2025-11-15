using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step5;

public class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;
    [Inject] protected HttpClient Http { get; set; } = default!;

    protected bool isComplete;
    protected bool isLoading = false;
    protected string errorMessage = string.Empty;
    protected List<PokemonListItem> allPokemon = new();
    protected List<PokemonListItem> currentPagePokemon = new();
    
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages = 0;

    private bool _disposed = false;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 5);
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
                totalPages = (int)Math.Ceiling(allPokemon.Count / (double)pageSize);
                UpdateCurrentPage();
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

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("pokemon", 5);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 5);
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
}
