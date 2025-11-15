using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step3;

public partial class ExampleBase : ComponentBase, IDisposable
{
    // Dependency Injection
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    // State
    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
    protected List<LivePokemonItem>? livePagedPokemon;
    protected int liveCurrentPage = 1;
    protected int livePageSize = 20;
    protected int liveTotalPages = 1;

    // Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 3);
        isComplete = step?.IsComplete ?? false;

        // Load demo data
        try
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            var response = await client.GetFromJsonAsync<PokeListDto>("pokemon?limit=151");

            if (response?.results is not null)
            {
                livePagedPokemon = response.results
                    .Select(p => new LivePokemonItem
                    {
                        Name = p.name ?? string.Empty
                    })
                    .ToList();

                liveTotalPages = Math.Max(1, (int)Math.Ceiling(livePagedPokemon.Count / (double)livePageSize));
            }
            else
            {
                livePagedPokemon = new List<LivePokemonItem>();
                liveTotalPages = 1;
            }
        }
        catch
        {
            livePagedPokemon = new List<LivePokemonItem>();
            liveTotalPages = 1;
        }
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
        await ProgressService.MarkStepCompleteAsync("pokemon", 3);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 3);
        isComplete = false;
    }

    // Helper Methods
    protected List<LivePokemonItem> GetLivePagedPokemon()
    {
        if (livePagedPokemon is null || livePagedPokemon.Count == 0)
        {
            return new List<LivePokemonItem>();
        }

        return livePagedPokemon
            .Skip((liveCurrentPage - 1) * livePageSize)
            .Take(livePageSize)
            .ToList();
    }

    protected void LiveNextPage()
    {
        if (liveCurrentPage < liveTotalPages)
        {
            liveCurrentPage++;
        }
    }

    protected void LivePreviousPage()
    {
        if (liveCurrentPage > 1)
        {
            liveCurrentPage--;
        }
    }

    // Cleanup
    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }

    // DTOs for live demo
    protected sealed class PokeListDto
    {
        public List<PokeItemDto> results { get; set; } = new();
    }

    protected sealed class PokeItemDto
    {
        public string? name { get; set; }
        public string? url { get; set; }
    }

    protected sealed class LivePokemonItem
    {
        public string Name { get; set; } = string.Empty;
    }
}
