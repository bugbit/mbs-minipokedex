using System.Text.Json;
using MiniPokedex.Domain.Models;
using MiniPokedex.Domain.Ports;
using MiniPokedex.Infrastructure.PokeApi.Models;

namespace MiniPokedex.Infrastructure.PokeApi;

/// <summary>
/// Implements <see cref="IPokemonRepository"/> using the PokéAPI HTTP client.
/// Responsible for fetching raw API responses and mapping them to domain models.
/// All mapping logic (sprites, stat labels, type ordering) is contained here,
/// keeping both the Domain and Application layers free of API-specific concerns.
/// </summary>
public sealed class PokeApiPokemonRepository(IPokeApiClient client) : IPokemonRepository
{
    /// <inheritdoc/>
    public async Task<(IReadOnlyList<PokemonSummary> Items, int TotalCount)> ListPageAsync(
        int page, int pageSize, CancellationToken ct)
    {
        var list = await client.ListPokemonAsync(pageSize, (page - 1) * pageSize, ct);
        var fetched = await Task.WhenAll(list.Results.Select(r => client.GetPokemonAsync(r.Name, ct)));

        var items = fetched
            .Where(p => p is not null)
            .Select(p => ToSummary(p!))
            .OrderBy(s => s.Id)
            .ToList();

        return (items, list.Count);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<PokemonSummary> Items, int TotalCount)> SearchPageAsync(
        string term, int page, int pageSize, CancellationToken ct)
    {
        if (int.TryParse(term, out var id))
        {
            // Búsqueda exacta por número de Pokédex
            var pokemon = await client.GetPokemonAsync(id.ToString(), ct);
            IReadOnlyList<PokemonSummary> exactResult = pokemon is null
                ? []
                : [ToSummary(pokemon)];
            return (exactResult, exactResult.Count);
        }

        // Búsqueda parcial por nombre:
        //   1. Una sola llamada ligera devuelve todos los nombres (~1300 sin imágenes).
        //   2. Se filtra en memoria y se cuenta el total de coincidencias.
        //   3. Solo se hace fetch detallado de los elementos de la página actual.
        var allNames = await client.ListPokemonAsync(2000, 0, ct);
        var allMatches = allNames.Results.Where(r => r.Name.Contains(term)).ToList();

        var pageSlice = allMatches.Skip((page - 1) * pageSize).Take(pageSize);
        var fetched = await Task.WhenAll(pageSlice.Select(r => client.GetPokemonAsync(r.Name, ct)));

        var items = fetched
            .Where(p => p is not null)
            .Select(p => ToSummary(p!))
            .OrderBy(s => s.Id)
            .ToList();

        return (items, allMatches.Count);
    }

    /// <inheritdoc/>
    public async Task<PokemonDetail?> GetDetailAsync(string idOrName, CancellationToken ct)
    {
        var pokemon = await client.GetPokemonAsync(idOrName, ct);
        return pokemon is null ? null : ToDetail(pokemon);
    }

    // ── Mapping helpers ──────────────────────────────────────────────────────

    /// <summary>Maps a raw API <see cref="Pokemon"/> to a <see cref="PokemonSummary"/>.</summary>
    private static PokemonSummary ToSummary(Pokemon p) => new(
        p.Id,
        p.Name,
        GetSpriteUrl(p.Sprites),
        p.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList());

    /// <summary>Maps a raw API <see cref="Pokemon"/> to a <see cref="PokemonDetail"/>.</summary>
    private static PokemonDetail ToDetail(Pokemon p) => new(
        p.Id,
        p.Name,
        p.Height,
        p.Weight,
        p.BaseExperience,
        GetSpriteUrl(p.Sprites),
        GetSpriteUrl(p.Sprites, shiny: true),
        p.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
        p.Stats.Select(s => (StatLabel(s.Stat.Name), s.BaseStat)).ToList(),
        p.Abilities.OrderBy(a => a.Slot).Select(a => (a.Ability.Name, a.IsHidden)).ToList());

    /// <summary>
    /// Resolves the sprite URL from the raw sprites JSON, preferring official artwork.
    /// Falls back to the standard front sprite when artwork is unavailable.
    /// </summary>
    /// <param name="sprites">Raw JSON element from the PokéAPI sprites object.</param>
    /// <param name="shiny">When <c>true</c>, returns the shiny variant URL.</param>
    private static string? GetSpriteUrl(JsonElement sprites, bool shiny = false)
    {
        var key = shiny ? "front_shiny" : "front_default";

        if (sprites.TryGetProperty("other", out var other) &&
            other.TryGetProperty("official-artwork", out var artwork) &&
            artwork.TryGetProperty(key, out var art) &&
            art.ValueKind == JsonValueKind.String)
            return art.GetString();

        if (sprites.TryGetProperty(key, out var fallback) &&
            fallback.ValueKind == JsonValueKind.String)
            return fallback.GetString();

        return null;
    }

    /// <summary>Maps PokéAPI internal stat names to human-readable display labels.</summary>
    private static string StatLabel(string name) => name switch
    {
        "hp"              => "HP",
        "attack"          => "Attack",
        "defense"         => "Defense",
        "special-attack"  => "Sp. Atk",
        "special-defense" => "Sp. Def",
        "speed"           => "Speed",
        _                 => name
    };
}
