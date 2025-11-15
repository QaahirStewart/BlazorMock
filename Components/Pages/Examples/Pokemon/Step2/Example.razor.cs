using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step2;

public partial class ExampleBase : ComponentBase, IDisposable
{
    [Inject] protected ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected bool isComplete;
    protected IJSObjectReference? _copyModule;
    protected List<LivePokemonItem>? livePokemon;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync("pokemon", 2);
        isComplete = step?.IsComplete ?? false;

        try
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            var response = await client.GetFromJsonAsync<PokeListDto>("pokemon?limit=8");

            if (response?.results is not null)
            {
                livePokemon = response.results
                    .Take(8)
                    .Select((p, index) => new LivePokemonItem
                    {
                        Name = p.name ?? string.Empty,
                        SpriteUrl = BuildSpriteUrlFromUrl(p.url)
                    })
                    .ToList();
            }
            else
            {
                livePokemon = new List<LivePokemonItem>();
            }
        }
        catch
        {
            livePokemon = new List<LivePokemonItem>();
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

    protected async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync("pokemon", 2);
        isComplete = true;
    }

    protected async Task ResetStep()
    {
        await ProgressService.MarkStepIncompleteAsync("pokemon", 2);
        isComplete = false;
    }

    protected static string? BuildSpriteUrlFromUrl(string? apiUrl)
    {
        if (string.IsNullOrWhiteSpace(apiUrl)) return null;

        // Example: https://pokeapi.co/api/v2/pokemon/25/
        var parts = apiUrl.TrimEnd('/').Split('/');
        var idPart = parts.LastOrDefault();
        if (!int.TryParse(idPart, out var id)) return null;

        return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
    }

    public void Dispose()
    {
        _copyModule?.DisposeAsync();
    }

    // Minimal DTOs just for the live demo
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
        public string? SpriteUrl { get; set; }
    }
}
