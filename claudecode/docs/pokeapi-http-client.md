# PokéAPI Typed HTTP Client

## Summary

A fully typed C# HTTP client for the [PokéAPI v2](https://pokeapi.co/api/v2/) REST API, generated from the OpenAPI spec at `context/pokeapi-openapi.json`. Registered in the ASP.NET Core DI container as a typed `HttpClient`.

## Files

| File | Purpose |
|------|---------|
| `Infrastructure/PokeApi/Models/PokeApiModels.cs` | C# records for all OpenAPI schemas |
| `Infrastructure/PokeApi/IPokeApiClient.cs` | Service interface |
| `Infrastructure/PokeApi/PokeApiClient.cs` | `HttpClient`-backed implementation |
| `Program.cs` | DI registration |

## Models

All models are immutable `record` types. JSON deserialization uses `JsonNamingPolicy.SnakeCaseLower` globally — no `[JsonPropertyName]` attributes needed.

| C# Record | Maps to API schema | Notes |
|---|---|---|
| `NamedAPIResource` | `NamedAPIResource` | `{ Name, Url }` |
| `APIResource` | `APIResource` | `{ Url }` |
| `NamedAPIResourceList` | `NamedAPIResourceList` | Paginated list with named resources |
| `APIResourceList` | `APIResourceList` | Paginated list with anonymous resources |
| `Pokemon` | `Pokemon` | `Sprites` is `JsonElement` (free-form object) |
| `PokemonAbility` | inline in `Pokemon.abilities` | |
| `PokemonStat` | inline in `Pokemon.stats` | |
| `PokemonTypeSlot` | inline in `Pokemon.types` | |
| `Ability` | `Ability` | |
| `AbilityEffectEntry` | inline in `Ability.effect_entries` | |
| `PokeType` | `Type` | Named `PokeType` to avoid conflict with `System.Type`; `DamageRelations` is `JsonElement` |
| `TypePokemonSlot` | inline in `Type.pokemon` | |
| `Move` | `Move` | |
| `Item` | `Item` | |
| `Berry` | `Berry` | |
| `EvolutionChain` | `EvolutionChain` | `Chain` is `JsonElement` (recursive free-form tree) |
| `Machine` | `Machine` | |
| `ContestType` | `ContestType` | |
| `Location` | `Location` | |
| `PokemonSpecies` | `PokemonSpecies` | |
| `Language` | `Language` | |

## Interface — `IPokeApiClient`

24 methods, following the pattern `List*Async` / `Get*Async` per resource group:

```csharp
// Pokémon
Task<NamedAPIResourceList> ListPokemonAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
Task<Pokemon?> GetPokemonAsync(string idOrName, CancellationToken ct = default);

// Abilities
Task<NamedAPIResourceList> ListAbilitiesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
Task<Ability?> GetAbilityAsync(string idOrName, CancellationToken ct = default);

// Types
Task<NamedAPIResourceList> ListTypesAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
Task<PokeType?> GetTypeAsync(string idOrName, CancellationToken ct = default);

// Moves / Items / Berries — same pattern
// Evolution chains / Machines — int id instead of string idOrName
// Locations / Species / Contest types / Languages — same pattern
```

Single-resource methods (`Get*Async`) return `null` on HTTP 404 and throw `HttpRequestException` on other errors.

## Implementation details

- Configured as a **typed `HttpClient`** — `AddHttpClient<IPokeApiClient, PokeApiClient>`.
- Base address: `https://pokeapi.co/api/v2/`.
- Deserialization via `System.Net.Http.Json.ReadFromJsonAsync<T>` (included in the ASP.NET Core SDK, no extra package needed).
- Two private helpers centralize all HTTP logic:

```csharp
// For list endpoints — always expects 200
private async Task<T> ListAsync<T>(string path, CancellationToken ct);

// For single-resource endpoints — returns default on 404
private async Task<T?> GetAsync<T>(string path, CancellationToken ct);
```

## DI Registration (`Program.cs`)

```csharp
builder.Services.AddHttpClient<IPokeApiClient, PokeApiClient>(client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
});
```

## Usage example

Inject `IPokeApiClient` in any controller or service:

```csharp
public class PokemonController(IPokeApiClient pokeApi) : Controller
{
    public async Task<IActionResult> Detail(string name)
    {
        var pokemon = await pokeApi.GetPokemonAsync(name);
        if (pokemon is null) return NotFound();
        return View(pokemon);
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 20;
        var list = await pokeApi.ListPokemonAsync(limit: pageSize, offset: (page - 1) * pageSize);
        return View(list);
    }
}
```

## Source

- OpenAPI spec: `context/pokeapi-openapi.json` (unofficial, hand-crafted, covers only GET operations)
- PokéAPI public documentation: https://pokeapi.co/docs/v2
