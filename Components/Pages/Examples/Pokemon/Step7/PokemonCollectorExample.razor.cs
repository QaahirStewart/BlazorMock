using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step7;

public class PokemonCollectorExampleBase : ComponentBase
{
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    protected bool isLoading = true;
    protected string errorMessage = string.Empty;
    protected List<PokemonListItem> allPokemon = new();
    protected List<PokemonListItem> filteredPokemon = new();
    protected PokemonDetail? selectedPokemon = null;
    protected string searchQuery = "";
    protected string selectedType = "";
    
    protected int currentPage = 1;
    protected int pageSize = 20;
    protected int totalPages => (int)Math.Ceiling(filteredPokemon.Count / (double)pageSize);

    protected List<PokemonListItem> CurrentPagePokemon => filteredPokemon
        .Skip((currentPage - 1) * pageSize)
        .Take(pageSize)
        .ToList();

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
                allPokemon.Clear();
                foreach (var result in response.Results)
                {
                    var id = int.Parse(result.Url.TrimEnd('/').Split('/').Last());
                    allPokemon.Add(new PokemonListItem
                    {
                        Id = id,
                        Name = result.Name,
                        Url = result.Url,
                        SpriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png"
                    });
                }

                // Load types for first 50 Pokemon for filtering
                var typeTasks = allPokemon.Take(50).Select(async p =>
                {
                    try
                    {
                        var details = await http.GetFromJsonAsync<PokemonDetailResponse>(p.Url);
                        if (details?.Types != null)
                        {
                            p.Types = details.Types.Select(t => t.Type.Name).ToList();
                        }
                    }
                    catch { }
                });
                await Task.WhenAll(typeTasks);

                ApplyFilters();
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

    protected void ApplyFilters()
    {
        filteredPokemon = allPokemon.Where(p =>
        {
            var matchesSearch = string.IsNullOrWhiteSpace(searchQuery) ||
                p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
            
            var matchesType = string.IsNullOrWhiteSpace(selectedType) ||
                (p.Types?.Contains(selectedType) ?? false);
            
            return matchesSearch && matchesType;
        }).ToList();

        currentPage = 1;
        StateHasChanged();
    }

    protected void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
        }
    }

    protected void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
        }
    }

    protected async Task ShowDetails(int id)
    {
        isLoading = true;
        try
        {
            var http = HttpClientFactory.CreateClient("PokeApi");
            var details = await http.GetFromJsonAsync<PokemonDetailResponse>(
                $"https://pokeapi.co/api/v2/pokemon/{id}");

            if (details != null)
            {
                selectedPokemon = new PokemonDetail
                {
                    Id = details.Id,
                    Name = details.Name,
                    SpriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png",
                    Height = details.Height,
                    Weight = details.Weight,
                    BaseExperience = details.BaseExperience,
                    Types = details.Types?.Select(t => t.Type.Name).ToList() ?? new(),
                    Abilities = details.Abilities?.Select(a => a.Ability.Name).ToList() ?? new(),
                    Hp = details.Stats?.FirstOrDefault(s => s.Stat.Name == "hp")?.BaseStat ?? 0,
                    Attack = details.Stats?.FirstOrDefault(s => s.Stat.Name == "attack")?.BaseStat ?? 0,
                    Defense = details.Stats?.FirstOrDefault(s => s.Stat.Name == "defense")?.BaseStat ?? 0,
                    Speed = details.Stats?.FirstOrDefault(s => s.Stat.Name == "speed")?.BaseStat ?? 0
                };
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load Pokemon details: {ex.Message}";
        }
        finally
        {
            isLoading = false;
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
        return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
    }

    protected string GetTypeBadgeClass(string type) => type.ToLower() switch
    {
        "fire" => "px-2.5 py-1 bg-red-50 text-red-700 border border-red-200 rounded-md text-xs font-medium capitalize",
        "water" => "px-2.5 py-1 bg-blue-50 text-blue-700 border border-blue-200 rounded-md text-xs font-medium capitalize",
        "grass" => "px-2.5 py-1 bg-green-50 text-green-700 border border-green-200 rounded-md text-xs font-medium capitalize",
        "electric" => "px-2.5 py-1 bg-yellow-50 text-yellow-700 border border-yellow-200 rounded-md text-xs font-medium capitalize",
        "ice" => "px-2.5 py-1 bg-cyan-50 text-cyan-700 border border-cyan-200 rounded-md text-xs font-medium capitalize",
        "fighting" => "px-2.5 py-1 bg-orange-50 text-orange-700 border border-orange-200 rounded-md text-xs font-medium capitalize",
        "poison" => "px-2.5 py-1 bg-purple-50 text-purple-700 border border-purple-200 rounded-md text-xs font-medium capitalize",
        "ground" => "px-2.5 py-1 bg-amber-50 text-amber-700 border border-amber-200 rounded-md text-xs font-medium capitalize",
        "flying" => "px-2.5 py-1 bg-indigo-50 text-indigo-700 border border-indigo-200 rounded-md text-xs font-medium capitalize",
        "psychic" => "px-2.5 py-1 bg-pink-50 text-pink-700 border border-pink-200 rounded-md text-xs font-medium capitalize",
        "bug" => "px-2.5 py-1 bg-lime-50 text-lime-700 border border-lime-200 rounded-md text-xs font-medium capitalize",
        "rock" => "px-2.5 py-1 bg-stone-50 text-stone-700 border border-stone-200 rounded-md text-xs font-medium capitalize",
        "ghost" => "px-2.5 py-1 bg-violet-50 text-violet-700 border border-violet-200 rounded-md text-xs font-medium capitalize",
        "dragon" => "px-2.5 py-1 bg-indigo-50 text-indigo-700 border border-indigo-300 rounded-md text-xs font-medium capitalize",
        "dark" => "px-2.5 py-1 bg-gray-800 text-gray-100 border border-gray-700 rounded-md text-xs font-medium capitalize",
        "steel" => "px-2.5 py-1 bg-slate-50 text-slate-700 border border-slate-200 rounded-md text-xs font-medium capitalize",
        "fairy" => "px-2.5 py-1 bg-pink-50 text-pink-700 border border-pink-300 rounded-md text-xs font-medium capitalize",
        "normal" => "px-2.5 py-1 bg-gray-100 text-gray-700 border border-gray-200 rounded-md text-xs font-medium capitalize",
        _ => "px-2.5 py-1 bg-gray-100 text-gray-700 border border-gray-200 rounded-md text-xs font-medium capitalize"
    };

    protected string GetTypeBackgroundColor(string type) => type.ToLower() switch
    {
        "grass" => "bg-gradient-to-br from-green-300 to-green-200",
        "poison" => "bg-gradient-to-br from-purple-300 to-purple-200",
        "fire" => "bg-gradient-to-br from-red-300 to-orange-200",
        "water" => "bg-gradient-to-br from-blue-300 to-blue-200",
        "electric" => "bg-gradient-to-br from-yellow-300 to-yellow-200",
        "ice" => "bg-gradient-to-br from-cyan-300 to-cyan-200",
        "fighting" => "bg-gradient-to-br from-orange-400 to-orange-300",
        "ground" => "bg-gradient-to-br from-amber-400 to-amber-300",
        "flying" => "bg-gradient-to-br from-indigo-300 to-indigo-200",
        "psychic" => "bg-gradient-to-br from-pink-300 to-pink-200",
        "bug" => "bg-gradient-to-br from-lime-300 to-lime-200",
        "rock" => "bg-gradient-to-br from-stone-400 to-stone-300",
        "ghost" => "bg-gradient-to-br from-violet-400 to-violet-300",
        "dragon" => "bg-gradient-to-br from-indigo-400 to-purple-300",
        "dark" => "bg-gradient-to-br from-gray-700 to-gray-600",
        "steel" => "bg-gradient-to-br from-slate-400 to-slate-300",
        "fairy" => "bg-gradient-to-br from-pink-300 to-pink-200",
        "normal" => "bg-gradient-to-br from-gray-300 to-gray-200",
        _ => "bg-gradient-to-br from-gray-300 to-gray-200"
    };

    protected string GetWeaknessChipClass(string type) => type.ToLower() switch
    {
        "fire" => "px-3 py-1.5 bg-orange-500 text-white rounded-lg text-sm font-medium capitalize",
        "ice" => "px-3 py-1.5 bg-cyan-400 text-white rounded-lg text-sm font-medium capitalize",
        "psychic" => "px-3 py-1.5 bg-pink-500 text-white rounded-lg text-sm font-medium capitalize",
        "flying" => "px-3 py-1.5 bg-indigo-400 text-white rounded-lg text-sm font-medium capitalize",
        "water" => "px-3 py-1.5 bg-blue-500 text-white rounded-lg text-sm font-medium capitalize",
        "grass" => "px-3 py-1.5 bg-green-500 text-white rounded-lg text-sm font-medium capitalize",
        "electric" => "px-3 py-1.5 bg-yellow-500 text-white rounded-lg text-sm font-medium capitalize",
        "ground" => "px-3 py-1.5 bg-amber-600 text-white rounded-lg text-sm font-medium capitalize",
        "rock" => "px-3 py-1.5 bg-stone-600 text-white rounded-lg text-sm font-medium capitalize",
        "fighting" => "px-3 py-1.5 bg-orange-600 text-white rounded-lg text-sm font-medium capitalize",
        "bug" => "px-3 py-1.5 bg-lime-600 text-white rounded-lg text-sm font-medium capitalize",
        "poison" => "px-3 py-1.5 bg-purple-500 text-white rounded-lg text-sm font-medium capitalize",
        "ghost" => "px-3 py-1.5 bg-violet-600 text-white rounded-lg text-sm font-medium capitalize",
        "dark" => "px-3 py-1.5 bg-gray-800 text-white rounded-lg text-sm font-medium capitalize",
        "steel" => "px-3 py-1.5 bg-slate-600 text-white rounded-lg text-sm font-medium capitalize",
        "fairy" => "px-3 py-1.5 bg-pink-500 text-white rounded-lg text-sm font-medium capitalize",
        "dragon" => "px-3 py-1.5 bg-indigo-600 text-white rounded-lg text-sm font-medium capitalize",
        _ => "px-3 py-1.5 bg-gray-500 text-white rounded-lg text-sm font-medium capitalize"
    };

    protected List<string> GetTypeWeaknesses(string type) => type.ToLower() switch
    {
        "grass" => new List<string> { "Fire", "Ice", "Poison", "Flying", "Bug" },
        "poison" => new List<string> { "Ground", "Psychic" },
        "fire" => new List<string> { "Water", "Ground", "Rock" },
        "water" => new List<string> { "Electric", "Grass" },
        "electric" => new List<string> { "Ground" },
        "ice" => new List<string> { "Fire", "Fighting", "Rock", "Steel" },
        "fighting" => new List<string> { "Flying", "Psychic", "Fairy" },
        "ground" => new List<string> { "Water", "Grass", "Ice" },
        "flying" => new List<string> { "Electric", "Ice", "Rock" },
        "psychic" => new List<string> { "Bug", "Ghost", "Dark" },
        "bug" => new List<string> { "Fire", "Flying", "Rock" },
        "rock" => new List<string> { "Water", "Grass", "Fighting", "Ground", "Steel" },
        "ghost" => new List<string> { "Ghost", "Dark" },
        "dragon" => new List<string> { "Ice", "Dragon", "Fairy" },
        "dark" => new List<string> { "Fighting", "Bug", "Fairy" },
        "steel" => new List<string> { "Fire", "Fighting", "Ground" },
        "fairy" => new List<string> { "Poison", "Steel" },
        "normal" => new List<string> { "Fighting" },
        _ => new List<string> { }
    };

    protected List<int> GetEvolutionChain(int pokemonId) => pokemonId switch
    {
        1 => new List<int> { 1, 2, 3 },
        2 => new List<int> { 1, 2, 3 },
        3 => new List<int> { 1, 2, 3 },
        4 => new List<int> { 4, 5, 6 },
        5 => new List<int> { 4, 5, 6 },
        6 => new List<int> { 4, 5, 6 },
        7 => new List<int> { 7, 8, 9 },
        8 => new List<int> { 7, 8, 9 },
        9 => new List<int> { 7, 8, 9 },
        10 => new List<int> { 10, 11, 12 },
        11 => new List<int> { 10, 11, 12 },
        12 => new List<int> { 10, 11, 12 },
        13 => new List<int> { 13, 14, 15 },
        14 => new List<int> { 13, 14, 15 },
        15 => new List<int> { 13, 14, 15 },
        16 => new List<int> { 16, 17, 18 },
        17 => new List<int> { 16, 17, 18 },
        18 => new List<int> { 16, 17, 18 },
        19 => new List<int> { 19, 20 },
        20 => new List<int> { 19, 20 },
        21 => new List<int> { 21, 22 },
        22 => new List<int> { 21, 22 },
        23 => new List<int> { 23, 24 },
        24 => new List<int> { 23, 24 },
        25 => new List<int> { 25, 26 },
        26 => new List<int> { 25, 26 },
        27 => new List<int> { 27, 28 },
        28 => new List<int> { 27, 28 },
        29 => new List<int> { 29, 30, 31 },
        30 => new List<int> { 29, 30, 31 },
        31 => new List<int> { 29, 30, 31 },
        32 => new List<int> { 32, 33, 34 },
        33 => new List<int> { 32, 33, 34 },
        34 => new List<int> { 32, 33, 34 },
        35 => new List<int> { 35, 36 },
        36 => new List<int> { 35, 36 },
        37 => new List<int> { 37, 38 },
        38 => new List<int> { 37, 38 },
        39 => new List<int> { 39, 40 },
        40 => new List<int> { 39, 40 },
        41 => new List<int> { 41, 42 },
        42 => new List<int> { 41, 42 },
        43 => new List<int> { 43, 44, 45 },
        44 => new List<int> { 43, 44, 45 },
        45 => new List<int> { 43, 44, 45 },
        46 => new List<int> { 46, 47 },
        47 => new List<int> { 46, 47 },
        48 => new List<int> { 48, 49 },
        49 => new List<int> { 48, 49 },
        50 => new List<int> { 50, 51 },
        51 => new List<int> { 50, 51 },
        52 => new List<int> { 52, 53 },
        53 => new List<int> { 52, 53 },
        54 => new List<int> { 54, 55 },
        55 => new List<int> { 54, 55 },
        56 => new List<int> { 56, 57 },
        57 => new List<int> { 56, 57 },
        58 => new List<int> { 58, 59 },
        59 => new List<int> { 58, 59 },
        60 => new List<int> { 60, 61, 62 },
        61 => new List<int> { 60, 61, 62 },
        62 => new List<int> { 60, 61, 62 },
        63 => new List<int> { 63, 64, 65 },
        64 => new List<int> { 63, 64, 65 },
        65 => new List<int> { 63, 64, 65 },
        66 => new List<int> { 66, 67, 68 },
        67 => new List<int> { 66, 67, 68 },
        68 => new List<int> { 66, 67, 68 },
        69 => new List<int> { 69, 70, 71 },
        70 => new List<int> { 69, 70, 71 },
        71 => new List<int> { 69, 70, 71 },
        72 => new List<int> { 72, 73 },
        73 => new List<int> { 72, 73 },
        74 => new List<int> { 74, 75, 76 },
        75 => new List<int> { 74, 75, 76 },
        76 => new List<int> { 74, 75, 76 },
        77 => new List<int> { 77, 78 },
        78 => new List<int> { 77, 78 },
        79 => new List<int> { 79, 80 },
        80 => new List<int> { 79, 80 },
        81 => new List<int> { 81, 82 },
        82 => new List<int> { 81, 82 },
        84 => new List<int> { 84, 85 },
        85 => new List<int> { 84, 85 },
        86 => new List<int> { 86, 87 },
        87 => new List<int> { 86, 87 },
        88 => new List<int> { 88, 89 },
        89 => new List<int> { 88, 89 },
        90 => new List<int> { 90, 91 },
        91 => new List<int> { 90, 91 },
        92 => new List<int> { 92, 93, 94 },
        93 => new List<int> { 92, 93, 94 },
        94 => new List<int> { 92, 93, 94 },
        95 => new List<int> { 95, 208 },
        96 => new List<int> { 96, 97 },
        97 => new List<int> { 96, 97 },
        98 => new List<int> { 98, 99 },
        99 => new List<int> { 98, 99 },
        100 => new List<int> { 100, 101 },
        101 => new List<int> { 100, 101 },
        102 => new List<int> { 102, 103 },
        103 => new List<int> { 102, 103 },
        104 => new List<int> { 104, 105 },
        105 => new List<int> { 104, 105 },
        106 => new List<int> { 106 },
        107 => new List<int> { 107 },
        108 => new List<int> { 108 },
        109 => new List<int> { 109, 110 },
        110 => new List<int> { 109, 110 },
        111 => new List<int> { 111, 112 },
        112 => new List<int> { 111, 112 },
        113 => new List<int> { 113 },
        114 => new List<int> { 114 },
        115 => new List<int> { 115 },
        116 => new List<int> { 116, 117 },
        117 => new List<int> { 116, 117 },
        118 => new List<int> { 118, 119 },
        119 => new List<int> { 118, 119 },
        120 => new List<int> { 120, 121 },
        121 => new List<int> { 120, 121 },
        123 => new List<int> { 123 },
        125 => new List<int> { 125 },
        126 => new List<int> { 126 },
        127 => new List<int> { 127 },
        128 => new List<int> { 128 },
        129 => new List<int> { 129, 130 },
        130 => new List<int> { 129, 130 },
        131 => new List<int> { 131 },
        133 => new List<int> { 133 },
        137 => new List<int> { 137 },
        138 => new List<int> { 138, 139 },
        139 => new List<int> { 138, 139 },
        140 => new List<int> { 140, 141 },
        141 => new List<int> { 140, 141 },
        142 => new List<int> { 142 },
        143 => new List<int> { 143 },
        144 => new List<int> { 144 },
        145 => new List<int> { 145 },
        146 => new List<int> { 146 },
        147 => new List<int> { 147, 148, 149 },
        148 => new List<int> { 147, 148, 149 },
        149 => new List<int> { 147, 148, 149 },
        150 => new List<int> { 150 },
        151 => new List<int> { 151 },
        _ => new List<int> { pokemonId }
    };

    // Data Models
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

    public class PokemonListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public string SpriteUrl { get; set; } = "";
        public List<string>? Types { get; set; }
    }

    public class PokemonDetailResponse
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
        public int BaseExperience { get; set; }

        [JsonPropertyName("types")]
        public List<PokemonTypeSlot> Types { get; set; } = new();

        [JsonPropertyName("abilities")]
        public List<PokemonAbilitySlot> Abilities { get; set; } = new();

        [JsonPropertyName("stats")]
        public List<PokemonStatSlot> Stats { get; set; } = new();
    }

    public class PokemonTypeSlot
    {
        [JsonPropertyName("type")]
        public TypeInfo Type { get; set; } = new();
    }

    public class TypeInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class PokemonAbilitySlot
    {
        [JsonPropertyName("ability")]
        public AbilityInfo Ability { get; set; } = new();
    }

    public class AbilityInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class PokemonStatSlot
    {
        [JsonPropertyName("base_stat")]
        public int BaseStat { get; set; }

        [JsonPropertyName("stat")]
        public StatInfo Stat { get; set; } = new();
    }

    public class StatInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class PokemonDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string SpriteUrl { get; set; } = "";
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public List<string> Types { get; set; } = new();
        public List<string> Abilities { get; set; } = new();
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
    }
}
