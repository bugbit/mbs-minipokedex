using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MiniPokedex.Infrastructure.PokeApi.Models;

namespace MiniPokedex.Infrastructure.PokeApi;

/// <summary>
/// HTTP implementation of <see cref="IPokeApiClient"/> that communicates with
/// PokéAPI v2. Registered as a typed <see cref="HttpClient"/> in DI so the base
/// address and lifetime are managed by <c>IHttpClientFactory</c>.
/// </summary>
public sealed class PokeApiClient : IPokeApiClient
{
    /// <summary>
    /// Shared JSON options: snake_case property names, case-insensitive matching.
    /// Static and reused across all requests to avoid repeated allocations.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _http;

    /// <summary>
    /// Initialises the client with the <see cref="HttpClient"/> provided by DI.
    /// </summary>
    /// <param name="http">Pre-configured HTTP client with the PokéAPI base address.</param>
    public PokeApiClient(HttpClient http) => _http = http;

    // ── Pokémon ──────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListPokemonAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"pokemon/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Pokemon?> GetPokemonAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Pokemon>($"pokemon/{idOrName}/", ct);

    // ── Abilities ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListAbilitiesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"ability/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Ability?> GetAbilityAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Ability>($"ability/{idOrName}/", ct);

    // ── Types ────────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"type/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<PokeType?> GetTypeAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<PokeType>($"type/{idOrName}/", ct);

    // ── Moves ────────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListMovesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"move/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Move?> GetMoveAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Move>($"move/{idOrName}/", ct);

    // ── Items ────────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListItemsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"item/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Item?> GetItemAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Item>($"item/{idOrName}/", ct);

    // ── Berries ──────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListBerriesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"berry/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Berry?> GetBerryAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Berry>($"berry/{idOrName}/", ct);

    // ── Evolution chains ─────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<APIResourceList> ListEvolutionChainsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<APIResourceList>($"evolution-chain/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<EvolutionChain?> GetEvolutionChainAsync(int id, CancellationToken ct = default)
        => GetAsync<EvolutionChain>($"evolution-chain/{id}/", ct);

    // ── Locations ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListLocationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"location/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Location?> GetLocationAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Location>($"location/{idOrName}/", ct);

    // ── Pokémon species ──────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListPokemonSpeciesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"pokemon-species/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<PokemonSpecies?> GetPokemonSpeciesAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<PokemonSpecies>($"pokemon-species/{idOrName}/", ct);

    // ── Machines ─────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<APIResourceList> ListMachinesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<APIResourceList>($"machine/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Machine?> GetMachineAsync(int id, CancellationToken ct = default)
        => GetAsync<Machine>($"machine/{id}/", ct);

    // ── Contest types ────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListContestTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"contest-type/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<ContestType?> GetContestTypeAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<ContestType>($"contest-type/{idOrName}/", ct);

    // ── Languages ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Task<NamedAPIResourceList> ListLanguagesAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
        => ListAsync<NamedAPIResourceList>($"language/?limit={limit}&offset={offset}", ct);

    /// <inheritdoc/>
    public Task<Language?> GetLanguageAsync(string idOrName, CancellationToken ct = default)
        => GetAsync<Language>($"language/{idOrName}/", ct);

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Fetches a list resource and throws on non-success status codes.
    /// </summary>
    /// <typeparam name="T">Deserialization target type.</typeparam>
    /// <param name="path">Relative path appended to the base address.</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task<T> ListAsync<T>(string path, CancellationToken ct)
    {
        var response = await _http.GetAsync(path, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    /// <summary>
    /// Fetches a single resource and returns <c>null</c> on 404, or throws on other errors.
    /// </summary>
    /// <typeparam name="T">Deserialization target type.</typeparam>
    /// <param name="path">Relative path appended to the base address.</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task<T?> GetAsync<T>(string path, CancellationToken ct)
    {
        var response = await _http.GetAsync(path, ct);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
    }
}
