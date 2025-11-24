using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Pokemon.Step7;

public partial class PokemonCollectorExample : ComponentBase
{
    [Inject] private HttpClient Http { get; set; } = default!;

    private bool isLoading = true;
    private string errorMessage = string.Empty;
    private List<PokemonListItem> allPokemon = new();
    private List<PokemonListItem> filteredPokemon = new();
    private PokemonDetail? selectedPokemon = null;
    private string searchQuery = "";
    private string selectedType = "";
    
    private int currentPage = 1;
    private int pageSize = 20;
    private int totalPages => (int)Math.Ceiling(filteredPokemon.Count / (double)pageSize);

    private List<PokemonListItem> CurrentPagePokemon => filteredPokemon
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
            var response = await Http.GetFromJsonAsync<PokemonListResponse>(
                "https://pokeapi.co/api/v2/pokemon?limit=151");

            if (response?.Results != null)
            {
                foreach (var result in response.Results)
                {
                    var id = int.Parse(result.Url.TrimEnd('/').Split('/').Last());
                    allPokemon.Add(new PokemonListItem
                    {
                        Id = id,
                        Name = result.Name,
                        SpriteUrl = GetSpriteUrl(id)
                    });
                }
                
                // Load types for first 50 Pokemon (for type filtering)
                var typeTasks = allPokemon.Take(50).Select(async p =>
                {
                    var apiResponse = await Http.GetFromJsonAsync<PokemonApiResponse>(
                        $"https://pokeapi.co/api/v2/pokemon/{p.Id}");
                    if (apiResponse?.Types != null)
                    {
                        p.Types = apiResponse.Types.Select(t => t.Type.Name).ToList();
                    }
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

    private void ApplyFilters()
    {
        filteredPokemon = allPokemon.Where(p =>
        {
            var matchesSearch = string.IsNullOrWhiteSpace(searchQuery) ||
                p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
            
            var matchesType = string.IsNullOrWhiteSpace(selectedType) ||
                (p.Types?.Any(t => t.Equals(selectedType, 
                    StringComparison.OrdinalIgnoreCase)) ?? false);
            
            return matchesSearch && matchesType;
        }).ToList();

        currentPage = 1;
    }

    private void PreviousPage()
    {
        if (currentPage > 1) currentPage--;
    }

    private void NextPage()
    {
        if (currentPage < totalPages) currentPage++;
    }

    private async Task ShowDetails(int id)
    {
        try
        {
            var apiResponse = await Http.GetFromJsonAsync<PokemonApiResponse>(
                $"https://pokeapi.co/api/v2/pokemon/{id}");
            if (apiResponse != null)
            {
                selectedPokemon = PokemonDetail.FromApiResponse(apiResponse);
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load details: {ex.Message}";
        }
    }

    private void CloseModal() => selectedPokemon = null;

    private static string GetSpriteUrl(int id) =>
        $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";

    // Type color mapping methods
    private string GetTypeBadgeClass(string type) => type.ToLower() switch
    {
        "fire" => "px-2.5 py-1 bg-red-50 text-red-700 border border-red-200 rounded-md text-xs font-medium capitalize",
        "water" => "px-2.5 py-1 bg-blue-50 text-blue-700 border border-blue-200 rounded-md text-xs font-medium capitalize",
        "grass" => "px-2.5 py-1 bg-green-50 text-green-700 border border-green-200 rounded-md text-xs font-medium capitalize",
        "electric" => "px-2.5 py-1 bg-yellow-50 text-yellow-700 border border-yellow-200 rounded-md text-xs font-medium capitalize",
        // ... add all 18 types
        _ => "px-2.5 py-1 bg-gray-100 text-gray-700 border border-gray-200 rounded-md text-xs font-medium capitalize"
    };

    private string GetTypeChipClass(string type) => type.ToLower() switch
    {
        "fire" => "px-4 py-1.5 bg-red-500 text-white rounded-full text-sm font-semibold capitalize",
        "water" => "px-4 py-1.5 bg-blue-500 text-white rounded-full text-sm font-semibold capitalize",
        "grass" => "px-4 py-1.5 bg-green-500 text-white rounded-full text-sm font-semibold capitalize",
        // ... add all 18 types
        _ => "px-4 py-1.5 bg-gray-500 text-white rounded-full text-sm font-semibold capitalize"
    };

    private string GetTypeBackgroundColor(string type) => type.ToLower() switch
    {
        "fire" => "bg-gradient-to-br from-red-300 to-orange-200",
        "water" => "bg-gradient-to-br from-blue-300 to-blue-200",
        "grass" => "bg-gradient-to-br from-green-300 to-green-200",
        "electric" => "bg-gradient-to-br from-yellow-300 to-yellow-200",
        // ... add all 18 types
        _ => "bg-gradient-to-br from-gray-300 to-gray-200"
    };

    private List<string> GetTypeWeaknesses(string type) => type.ToLower() switch
    {
        "grass" => new() { "Fire", "Ice", "Poison", "Flying", "Bug" },
        "fire" => new() { "Water", "Ground", "Rock" },
        "water" => new() { "Electric", "Grass" },
        "electric" => new() { "Ground" },
        "normal" => new() { "Fighting" },
        // ... add all 18 types
        _ => new() { "Fighting" }
    };

    private string GetWeaknessChipClass(string type) => type.ToLower() switch
    {
        "fire" => "px-3 py-1 bg-red-100 text-red-700 rounded-full text-xs font-medium capitalize",
        "water" => "px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-xs font-medium capitalize",
        "grass" => "px-3 py-1 bg-green-100 text-green-700 rounded-full text-xs font-medium capitalize",
        "electric" => "px-3 py-1 bg-yellow-100 text-yellow-700 rounded-full text-xs font-medium capitalize",
        "ice" => "px-3 py-1 bg-cyan-100 text-cyan-700 rounded-full text-xs font-medium capitalize",
        "fighting" => "px-3 py-1 bg-orange-100 text-orange-700 rounded-full text-xs font-medium capitalize",
        "poison" => "px-3 py-1 bg-purple-100 text-purple-700 rounded-full text-xs font-medium capitalize",
        "ground" => "px-3 py-1 bg-amber-100 text-amber-700 rounded-full text-xs font-medium capitalize",
        "flying" => "px-3 py-1 bg-indigo-100 text-indigo-700 rounded-full text-xs font-medium capitalize",
        "psychic" => "px-3 py-1 bg-pink-100 text-pink-700 rounded-full text-xs font-medium capitalize",
        "bug" => "px-3 py-1 bg-lime-100 text-lime-700 rounded-full text-xs font-medium capitalize",
        "rock" => "px-3 py-1 bg-stone-100 text-stone-700 rounded-full text-xs font-medium capitalize",
        "ghost" => "px-3 py-1 bg-violet-100 text-violet-700 rounded-full text-xs font-medium capitalize",
        "dragon" => "px-3 py-1 bg-indigo-100 text-indigo-700 rounded-full text-xs font-medium capitalize",
        "dark" => "px-3 py-1 bg-gray-200 text-gray-700 rounded-full text-xs font-medium capitalize",
        "steel" => "px-3 py-1 bg-slate-100 text-slate-700 rounded-full text-xs font-medium capitalize",
        "fairy" => "px-3 py-1 bg-pink-100 text-pink-700 rounded-full text-xs font-medium capitalize",
        _ => "px-3 py-1 bg-gray-100 text-gray-700 rounded-full text-xs font-medium capitalize"
    };

    private List<int> GetEvolutionChain(int pokemonId) => pokemonId switch
    {
        // Starters
        1 or 2 or 3 => new() { 1, 2, 3 },       // Bulbasaur -> Ivysaur -> Venusaur
        4 or 5 or 6 => new() { 4, 5, 6 },       // Charmander -> Charmeleon -> Charizard
        7 or 8 or 9 => new() { 7, 8, 9 },       // Squirtle -> Wartortle -> Blastoise
        
        // Bug types
        10 or 11 or 12 => new() { 10, 11, 12 }, // Caterpie -> Metapod -> Butterfree
        13 or 14 or 15 => new() { 13, 14, 15 }, // Weedle -> Kakuna -> Beedrill
        
        // Birds
        16 or 17 or 18 => new() { 16, 17, 18 }, // Pidgey -> Pidgeotto -> Pidgeot
        21 or 22 => new() { 21, 22 },           // Spearow -> Fearow
        
        // Rodents
        19 or 20 => new() { 19, 20 },           // Rattata -> Raticate
        25 or 26 => new() { 25, 26 },           // Pikachu -> Raichu
        27 or 28 => new() { 27, 28 },           // Sandshrew -> Sandslash
        
        // Nidoran lines
        29 or 30 or 31 => new() { 29, 30, 31 }, // Nidoran♀ -> Nidorina -> Nidoqueen
        32 or 33 or 34 => new() { 32, 33, 34 }, // Nidoran♂ -> Nidorino -> Nidoking
        
        // Fairy/Normal
        35 or 36 => new() { 35, 36 },           // Clefairy -> Clefable
        39 or 40 => new() { 39, 40 },           // Jigglypuff -> Wigglytuff
        
        // Vulpix & Zubat
        37 or 38 => new() { 37, 38 },           // Vulpix -> Ninetales
        41 or 42 => new() { 41, 42 },           // Zubat -> Golbat
        
        // Grass
        43 or 44 or 45 => new() { 43, 44, 45 }, // Oddish -> Gloom -> Vileplume
        69 or 70 or 71 => new() { 69, 70, 71 }, // Bellsprout -> Weepinbell -> Victreebel
        
        // Bug/Grass
        46 or 47 => new() { 46, 47 },           // Paras -> Parasect
        48 or 49 => new() { 48, 49 },           // Venonat -> Venomoth
        
        // Ground
        50 or 51 => new() { 50, 51 },           // Diglett -> Dugtrio
        
        // Meowth & Psyduck
        52 or 53 => new() { 52, 53 },           // Meowth -> Persian
        54 or 55 => new() { 54, 55 },           // Psyduck -> Golduck
        
        // Mankey & Growlithe
        56 or 57 => new() { 56, 57 },           // Mankey -> Primeape
        58 or 59 => new() { 58, 59 },           // Growlithe -> Arcanine
        
        // Poliwag line
        60 or 61 or 62 => new() { 60, 61, 62 }, // Poliwag -> Poliwhirl -> Poliwrath
        
        // Abra line
        63 or 64 or 65 => new() { 63, 64, 65 }, // Abra -> Kadabra -> Alakazam
        
        // Machop line
        66 or 67 or 68 => new() { 66, 67, 68 }, // Machop -> Machoke -> Machamp
        
        // Tentacool & Geodude
        72 or 73 => new() { 72, 73 },           // Tentacool -> Tentacruel
        74 or 75 or 76 => new() { 74, 75, 76 }, // Geodude -> Graveler -> Golem
        
        // Ponyta & Slowpoke
        77 or 78 => new() { 77, 78 },           // Ponyta -> Rapidash
        79 or 80 => new() { 79, 80 },           // Slowpoke -> Slowbro
        
        // Magnemite & Doduo
        81 or 82 => new() { 81, 82 },           // Magnemite -> Magneton
        84 or 85 => new() { 84, 85 },           // Doduo -> Dodrio
        
        // Seel & Grimer
        86 or 87 => new() { 86, 87 },           // Seel -> Dewgong
        88 or 89 => new() { 88, 89 },           // Grimer -> Muk
        
        // Shellder & Gastly
        90 or 91 => new() { 90, 91 },           // Shellder -> Cloyster
        92 or 93 or 94 => new() { 92, 93, 94 }, // Gastly -> Haunter -> Gengar
        
        // Drowzee & Krabby
        96 or 97 => new() { 96, 97 },           // Drowzee -> Hypno
        98 or 99 => new() { 98, 99 },           // Krabby -> Kingler
        
        // Voltorb & Exeggcute
        100 or 101 => new() { 100, 101 },       // Voltorb -> Electrode
        102 or 103 => new() { 102, 103 },       // Exeggcute -> Exeggutor
        
        // Cubone & Koffing
        104 or 105 => new() { 104, 105 },       // Cubone -> Marowak
        109 or 110 => new() { 109, 110 },       // Koffing -> Weezing
        
        // Rhyhorn & Chansey
        111 or 112 => new() { 111, 112 },       // Rhyhorn -> Rhydon
        
        // Horsea & Goldeen
        116 or 117 => new() { 116, 117 },       // Horsea -> Seadra
        118 or 119 => new() { 118, 119 },       // Goldeen -> Seaking
        
        // Staryu & Magikarp
        120 or 121 => new() { 120, 121 },       // Staryu -> Starmie
        129 or 130 => new() { 129, 130 },       // Magikarp -> Gyarados
        
        // Eevee evolutions
        133 or 134 => new() { 133, 134 },       // Eevee -> Vaporeon
        133 or 135 => new() { 133, 135 },       // Eevee -> Jolteon
        133 or 136 => new() { 133, 136 },       // Eevee -> Flareon
        
        // Omanyte & Kabuto
        138 or 139 => new() { 138, 139 },       // Omanyte -> Omastar
        140 or 141 => new() { 140, 141 },       // Kabuto -> Kabutops
        
        // Dratini line
        147 or 148 or 149 => new() { 147, 148, 149 }, // Dratini -> Dragonair -> Dragonite
        
        // Single stage Pokemon (Legendaries, etc.)
        83 => new() { 83 },                     // Farfetch'd
        95 => new() { 95 },                     // Onix
        106 => new() { 106 },                   // Hitmonlee
        107 => new() { 107 },                   // Hitmonchan
        108 => new() { 108 },                   // Lickitung
        113 => new() { 113 },                   // Chansey
        114 => new() { 114 },                   // Tangela
        115 => new() { 115 },                   // Kangaskhan
        122 => new() { 122 },                   // Mr. Mime
        123 => new() { 123 },                   // Scyther
        124 => new() { 124 },                   // Jynx
        125 => new() { 125 },                   // Electabuzz
        126 => new() { 126 },                   // Magmar
        127 => new() { 127 },                   // Pinsir
        128 => new() { 128 },                   // Tauros
        131 => new() { 131 },                   // Lapras
        132 => new() { 132 },                   // Ditto
        137 => new() { 137 },                   // Porygon
        142 => new() { 142 },                   // Aerodactyl
        143 => new() { 143 },                   // Snorlax
        144 => new() { 144 },                   // Articuno
        145 => new() { 145 },                   // Zapdos
        146 => new() { 146 },                   // Moltres
        150 => new() { 150 },                   // Mewtwo
        151 => new() { 151 },                   // Mew
        
        _ => new() { pokemonId }                // Fallback
    };

    // DTO Classes
    public class PokemonListResponse
    {
        public List<PokemonResult> Results { get; set; } = new();
    }

    public class PokemonResult
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }

    public class PokemonListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string SpriteUrl { get; set; } = "";
        public List<string>? Types { get; set; }
    }

    // API Response DTOs (matching PokeAPI structure)
    public class PokemonApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Base_experience { get; set; }
        public List<TypeSlot>? Types { get; set; }
        public List<AbilitySlot>? Abilities { get; set; }
        public PokemonSprites? Sprites { get; set; }
        public List<StatSlot>? Stats { get; set; }
    }

    public class TypeSlot
    {
        public int Slot { get; set; }
        public TypeInfo Type { get; set; } = new();
    }

    public class TypeInfo
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }

    public class AbilitySlot
    {
        public bool Is_hidden { get; set; }
        public int Slot { get; set; }
        public AbilityInfo Ability { get; set; } = new();
    }

    public class AbilityInfo
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }

    public class PokemonSprites
    {
        public string? Front_default { get; set; }
        public OtherSprites? Other { get; set; }
    }

    public class OtherSprites
    {
        public OfficialArtwork? Official_artwork { get; set; }
    }

    public class OfficialArtwork
    {
        public string? Front_default { get; set; }
    }

    public class StatSlot
    {
        public int Base_stat { get; set; }
        public int Effort { get; set; }
        public StatInfo Stat { get; set; } = new();
    }

    public class StatInfo
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }

    // View Model for display
    public class PokemonDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public List<string>? Types { get; set; }
        public List<string>? Abilities { get; set; }
        public string SpriteUrl { get; set; } = "";
        public int Hp { get; set; }
        public int Attack { get; set; }

        public static PokemonDetail FromApiResponse(PokemonApiResponse api)
        {
            return new PokemonDetail
            {
                Id = api.Id,
                Name = api.Name,
                Height = api.Height,
                Weight = api.Weight,
                BaseExperience = api.Base_experience,
                Types = api.Types?.Select(t => t.Type.Name).ToList(),
                Abilities = api.Abilities?.Select(a => a.Ability.Name).ToList(),
                SpriteUrl = api.Sprites?.Other?.Official_artwork?.Front_default 
                    ?? api.Sprites?.Front_default 
                    ?? GetSpriteUrl(api.Id),
                Hp = api.Stats?.FirstOrDefault(s => s.Stat.Name == "hp")?.Base_stat ?? 0,
                Attack = api.Stats?.FirstOrDefault(s => s.Stat.Name == "attack")?.Base_stat ?? 0
            };
        }

        private static string GetSpriteUrl(int id) =>
            $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
    }
}