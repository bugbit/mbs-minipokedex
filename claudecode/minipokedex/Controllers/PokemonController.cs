using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using minipokedex.Models;
using MiniPokedex.Infrastructure.PokeApi;
using MiniPokedex.Infrastructure.PokeApi.Models;

namespace minipokedex.Controllers;

/// <summary>
/// Handles the Pokédex pages: paginated list with search and individual Pokémon detail.
/// </summary>
public class PokemonController(IPokeApiClient pokeApi) : Controller
{
    /// <summary>Number of Pokémon cards shown per page in the list and search views.</summary>
    private const int PageSize = 20;

    /// <summary>
    /// Renders the Pokémon list. When <paramref name="search"/> is provided the list is
    /// filtered — by exact Pokédex number if the term is numeric, or by partial name otherwise —
    /// and the results are paginated with the same page size as the normal catalogue.
    /// </summary>
    /// <param name="page">1-based page number (defaults to 1).</param>
    /// <param name="search">Optional search term (name fragment or Pokédex number).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<IActionResult> Index(int page = 1, string? search = null, CancellationToken ct = default)
    {
        page = Math.Max(1, page);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLowerInvariant();
            var (cards, totalCount) = await SearchPageAsync(term, page, ct);
            return View(new PokemonListViewModel(cards, totalCount, page, PageSize, search.Trim()));
        }

        var list = await pokeApi.ListPokemonAsync(PageSize, (page - 1) * PageSize, ct);

        var fetched = await Task.WhenAll(list.Results.Select(r => pokeApi.GetPokemonAsync(r.Name, ct)));

        var pageCards = fetched
            .Where(p => p is not null)
            .Select(p => ToCard(p!))
            .OrderBy(c => c.Id)
            .ToList();

        return View(new PokemonListViewModel(pageCards, list.Count, page, PageSize));
    }

    /// <summary>
    /// Renders the detail page for a single Pokémon identified by name or ID.
    /// Returns 404 if the Pokémon does not exist in the PokéAPI.
    /// </summary>
    /// <param name="name">Pokémon name or numeric ID (case-insensitive).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<IActionResult> Detail(string name, CancellationToken ct = default)
    {
        var pokemon = await pokeApi.GetPokemonAsync(name.ToLowerInvariant(), ct);
        if (pokemon is null)
            return NotFound();

        var vm = new PokemonDetailViewModel(
            pokemon.Id,
            pokemon.Name,
            pokemon.Height,
            pokemon.Weight,
            pokemon.BaseExperience,
            GetSpriteUrl(pokemon.Sprites),
            GetSpriteUrl(pokemon.Sprites, shiny: true),
            pokemon.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
            pokemon.Stats.Select(s => (StatLabel(s.Stat.Name), s.BaseStat)).ToList(),
            pokemon.Abilities.OrderBy(a => a.Slot).Select(a => (a.Ability.Name, a.IsHidden)).ToList());

        return View(vm);
    }

    /// <summary>
    /// Filters the Pokémon catalogue by <paramref name="term"/> and returns a single page of results.
    /// <para>
    /// For numeric terms a direct API call by ID is made.
    /// For name fragments a single lightweight call retrieves all names, which are filtered
    /// in memory; only the slice for the requested page is then fetched in full detail.
    /// </para>
    /// </summary>
    /// <param name="term">Normalised (lowercase, trimmed) search term.</param>
    /// <param name="page">1-based page number within the search results.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The cards for the requested page and the total number of matching Pokémon.</returns>
    private async Task<(List<PokemonCardViewModel> Cards, int TotalCount)> SearchPageAsync(
        string term, int page, CancellationToken ct)
    {
        if (int.TryParse(term, out var id))
        {
            // Búsqueda exacta por número de Pokédex
            var pokemon = await pokeApi.GetPokemonAsync(id.ToString(), ct);
            var cards = pokemon is null
                ? []
                : (List<PokemonCardViewModel>)[ToCard(pokemon)];
            return (cards, cards.Count);
        }

        // Búsqueda parcial por nombre:
        //   1. Una sola llamada ligera devuelve todos los nombres (~1300 entradas sin imágenes).
        //   2. Se filtra en memoria y se cuentan los coincidentes.
        //   3. Solo se hace fetch detallado de los elementos de la página actual.
        var allNames = await pokeApi.ListPokemonAsync(2000, 0, ct);

        var allMatches = allNames.Results
            .Where(r => r.Name.Contains(term))
            .ToList();

        var pageSlice = allMatches
            .Skip((page - 1) * PageSize)
            .Take(PageSize);

        var fetched = await Task.WhenAll(pageSlice.Select(r => pokeApi.GetPokemonAsync(r.Name, ct)));

        var result = fetched
            .Where(p => p is not null)
            .Select(p => ToCard(p!))
            .OrderBy(c => c.Id)
            .ToList();

        return (result, allMatches.Count);
    }

    /// <summary>
    /// Builds a <see cref="PokemonCardViewModel"/> from a full <see cref="Pokemon"/> API response.
    /// </summary>
    private PokemonCardViewModel ToCard(Pokemon p) => new(
        p.Id,
        p.Name,
        GetSpriteUrl(p.Sprites),
        p.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList());

    /// <summary>
    /// Resolves the sprite URL for a Pokémon, preferring the official artwork.
    /// Falls back to the standard front sprite when artwork is unavailable.
    /// </summary>
    /// <param name="sprites">Raw JSON element from the PokéAPI sprites object.</param>
    /// <param name="shiny">When <c>true</c>, returns the shiny variant URL.</param>
    /// <returns>The sprite URL, or <c>null</c> if none is available.</returns>
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

    /// <summary>
    /// Maps PokéAPI internal stat names to human-readable display labels.
    /// </summary>
    /// <param name="name">Raw stat name as returned by the PokéAPI.</param>
    /// <returns>A short, display-friendly label.</returns>
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
