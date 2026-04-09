using MiniPokedex.Infrastructure.PokeApi.Models;

namespace MiniPokedex.Infrastructure.PokeApi;

/// <summary>
/// Typed client for the PokéAPI v2 (<c>https://pokeapi.co/api/v2/</c>).
/// Provides paginated list endpoints and single-resource fetch endpoints for
/// the most common PokéAPI domains. All methods are asynchronous and support
/// cooperative cancellation via <see cref="CancellationToken"/>.
/// </summary>
public interface IPokeApiClient
{
    // ── Pokémon ──────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of Pokémon resources.</summary>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="offset">Zero-based index of the first result.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<NamedAPIResourceList> ListPokemonAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single Pokémon by name or numeric ID, or <c>null</c> if not found.</summary>
    /// <param name="idOrName">Pokémon name (lowercase) or numeric ID as a string.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<Pokemon?> GetPokemonAsync(string idOrName, CancellationToken ct = default);

    // ── Abilities ────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of ability resources.</summary>
    Task<NamedAPIResourceList> ListAbilitiesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single ability by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Ability?> GetAbilityAsync(string idOrName, CancellationToken ct = default);

    // ── Types ────────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of Pokémon type resources.</summary>
    Task<NamedAPIResourceList> ListTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single type by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<PokeType?> GetTypeAsync(string idOrName, CancellationToken ct = default);

    // ── Moves ────────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of move resources.</summary>
    Task<NamedAPIResourceList> ListMovesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single move by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Move?> GetMoveAsync(string idOrName, CancellationToken ct = default);

    // ── Items ────────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of item resources.</summary>
    Task<NamedAPIResourceList> ListItemsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single item by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Item?> GetItemAsync(string idOrName, CancellationToken ct = default);

    // ── Berries ──────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of berry resources.</summary>
    Task<NamedAPIResourceList> ListBerriesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single berry by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Berry?> GetBerryAsync(string idOrName, CancellationToken ct = default);

    // ── Evolution chains ─────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of evolution-chain resources.</summary>
    Task<APIResourceList> ListEvolutionChainsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single evolution chain by numeric ID, or <c>null</c> if not found.</summary>
    Task<EvolutionChain?> GetEvolutionChainAsync(int id, CancellationToken ct = default);

    // ── Locations ────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of location resources.</summary>
    Task<NamedAPIResourceList> ListLocationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single location by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Location?> GetLocationAsync(string idOrName, CancellationToken ct = default);

    // ── Pokémon species ──────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of Pokémon species resources.</summary>
    Task<NamedAPIResourceList> ListPokemonSpeciesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single Pokémon species by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<PokemonSpecies?> GetPokemonSpeciesAsync(string idOrName, CancellationToken ct = default);

    // ── Machines ─────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of TM/HM machine resources.</summary>
    Task<APIResourceList> ListMachinesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single machine by numeric ID, or <c>null</c> if not found.</summary>
    Task<Machine?> GetMachineAsync(int id, CancellationToken ct = default);

    // ── Contest types ────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of contest-type resources.</summary>
    Task<NamedAPIResourceList> ListContestTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single contest type by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<ContestType?> GetContestTypeAsync(string idOrName, CancellationToken ct = default);

    // ── Languages ────────────────────────────────────────────────────────────

    /// <summary>Returns a paginated list of language resources.</summary>
    Task<NamedAPIResourceList> ListLanguagesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>Returns a single language by name or numeric ID, or <c>null</c> if not found.</summary>
    Task<Language?> GetLanguageAsync(string idOrName, CancellationToken ct = default);
}
