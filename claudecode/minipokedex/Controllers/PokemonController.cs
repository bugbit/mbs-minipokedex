using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using minipokedex.Models;
using MiniPokedex.Infrastructure.PokeApi;

namespace minipokedex.Controllers;

public class PokemonController(IPokeApiClient pokeApi) : Controller
{
    private const int PageSize = 20;

    public async Task<IActionResult> Index(int page = 1, string? search = null, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(search))
            return RedirectToAction(nameof(Detail), new { name = search.Trim().ToLowerInvariant() });

        page = Math.Max(1, page);
        var list = await pokeApi.ListPokemonAsync(PageSize, (page - 1) * PageSize, ct);

        var tasks = list.Results.Select(r => pokeApi.GetPokemonAsync(r.Name, ct));
        var fetched = await Task.WhenAll(tasks);

        var cards = fetched
            .Where(p => p is not null)
            .Select(p => new PokemonCardViewModel(
                p!.Id,
                p.Name,
                GetSpriteUrl(p.Sprites),
                p.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList()))
            .OrderBy(c => c.Id)
            .ToList();

        return View(new PokemonListViewModel(cards, list.Count, page, PageSize));
    }

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
