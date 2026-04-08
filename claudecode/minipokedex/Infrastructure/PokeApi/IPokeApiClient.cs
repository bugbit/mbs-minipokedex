using MiniPokedex.Infrastructure.PokeApi.Models;

namespace MiniPokedex.Infrastructure.PokeApi;

public interface IPokeApiClient
{
    // Pokémon
    Task<NamedAPIResourceList> ListPokemonAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Pokemon?> GetPokemonAsync(string idOrName, CancellationToken ct = default);

    // Abilities
    Task<NamedAPIResourceList> ListAbilitiesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Ability?> GetAbilityAsync(string idOrName, CancellationToken ct = default);

    // Types
    Task<NamedAPIResourceList> ListTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<PokeType?> GetTypeAsync(string idOrName, CancellationToken ct = default);

    // Moves
    Task<NamedAPIResourceList> ListMovesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Move?> GetMoveAsync(string idOrName, CancellationToken ct = default);

    // Items
    Task<NamedAPIResourceList> ListItemsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Item?> GetItemAsync(string idOrName, CancellationToken ct = default);

    // Berries
    Task<NamedAPIResourceList> ListBerriesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Berry?> GetBerryAsync(string idOrName, CancellationToken ct = default);

    // Evolution chains
    Task<APIResourceList> ListEvolutionChainsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<EvolutionChain?> GetEvolutionChainAsync(int id, CancellationToken ct = default);

    // Locations
    Task<NamedAPIResourceList> ListLocationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Location?> GetLocationAsync(string idOrName, CancellationToken ct = default);

    // Pokémon species
    Task<NamedAPIResourceList> ListPokemonSpeciesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<PokemonSpecies?> GetPokemonSpeciesAsync(string idOrName, CancellationToken ct = default);

    // Machines
    Task<APIResourceList> ListMachinesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Machine?> GetMachineAsync(int id, CancellationToken ct = default);

    // Contest types
    Task<NamedAPIResourceList> ListContestTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<ContestType?> GetContestTypeAsync(string idOrName, CancellationToken ct = default);

    // Languages
    Task<NamedAPIResourceList> ListLanguagesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Language?> GetLanguageAsync(string idOrName, CancellationToken ct = default);
}
