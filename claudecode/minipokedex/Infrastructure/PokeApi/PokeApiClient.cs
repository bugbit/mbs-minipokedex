using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MiniPokedex.Infrastructure.PokeApi.Models;

namespace MiniPokedex.Infrastructure.PokeApi;

public sealed class PokeApiClient : IPokeApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _http;

    public PokeApiClient(HttpClient http) => _http = http;

    // ── Pokémon ──────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListPokemonAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"pokemon/?limit={limit}&offset={offset}", ct);

    public Task<Pokemon?> GetPokemonAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Pokemon>($"pokemon/{idOrName}/", ct);

    // ── Abilities ────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListAbilitiesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"ability/?limit={limit}&offset={offset}", ct);

    public Task<Ability?> GetAbilityAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Ability>($"ability/{idOrName}/", ct);

    // ── Types ────────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"type/?limit={limit}&offset={offset}", ct);

    public Task<PokeType?> GetTypeAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<PokeType>($"type/{idOrName}/", ct);

    // ── Moves ────────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListMovesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"move/?limit={limit}&offset={offset}", ct);

    public Task<Move?> GetMoveAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Move>($"move/{idOrName}/", ct);

    // ── Items ────────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListItemsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"item/?limit={limit}&offset={offset}", ct);

    public Task<Item?> GetItemAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Item>($"item/{idOrName}/", ct);

    // ── Berries ──────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListBerriesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"berry/?limit={limit}&offset={offset}", ct);

    public Task<Berry?> GetBerryAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Berry>($"berry/{idOrName}/", ct);

    // ── Evolution chains ─────────────────────────────────────────────────────

    public Task<APIResourceList> ListEvolutionChainsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<APIResourceList>($"evolution-chain/?limit={limit}&offset={offset}", ct);

    public Task<EvolutionChain?> GetEvolutionChainAsync(int id, CancellationToken ct = default)
        => GetAsync<EvolutionChain>($"evolution-chain/{id}/", ct);

    // ── Locations ────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListLocationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"location/?limit={limit}&offset={offset}", ct);

    public Task<Location?> GetLocationAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Location>($"location/{idOrName}/", ct);

    // ── Pokémon species ──────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListPokemonSpeciesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"pokemon-species/?limit={limit}&offset={offset}", ct);

    public Task<PokemonSpecies?> GetPokemonSpeciesAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<PokemonSpecies>($"pokemon-species/{idOrName}/", ct);

    // ── Machines ─────────────────────────────────────────────────────────────

    public Task<APIResourceList> ListMachinesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<APIResourceList>($"machine/?limit={limit}&offset={offset}", ct);

    public Task<Machine?> GetMachineAsync(int id, CancellationToken ct = default)
        => GetAsync<Machine>($"machine/{id}/", ct);

    // ── Contest types ────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListContestTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"contest-type/?limit={limit}&offset={offset}", ct);

    public Task<ContestType?> GetContestTypeAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<ContestType>($"contest-type/{idOrName}/", ct);

    // ── Languages ────────────────────────────────────────────────────────────

    public Task<NamedAPIResourceList> ListLanguagesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"language/?limit={limit}&offset={offset}", ct);

    public Task<Language?> GetLanguageAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Language>($"language/{idOrName}/", ct);

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task<T> ListAsync<T>(string path, CancellationToken ct)
    {
        var response = await _http.GetAsync(path, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    private async Task<T?> GetAsync<T>(string path, CancellationToken ct)
    {
        var response = await _http.GetAsync(path, ct);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
    }
}
