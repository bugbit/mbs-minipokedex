using Microsoft.AspNetCore.Mvc;
using minipokedex.Models;
using MiniPokedex.Application.Services;

namespace minipokedex.Controllers;

/// <summary>
/// Handles Pokédex pages: paginated list with optional search, and individual detail.
/// Delegates all orchestration and data-fetching to <see cref="PokemonAppService"/>,
/// and is responsible only for mapping Application results to ViewModels and returning views.
/// </summary>
public class PokemonController(PokemonAppService pokemonService) : Controller
{
    /// <summary>Number of Pokémon cards shown per page.</summary>
    private const int PageSize = 20;

    /// <summary>
    /// Renders the Pokémon list page, optionally filtered by <paramref name="search"/>.
    /// </summary>
    /// <param name="page">1-based page number (defaults to 1).</param>
    /// <param name="search">Optional search term (name fragment or Pokédex number).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<IActionResult> Index(int page = 1, string? search = null, CancellationToken ct = default)
    {
        var (items, totalCount) = await pokemonService.GetPageAsync(page, PageSize, search, ct);

        var cards = items
            .Select(s => new PokemonCardViewModel(s.Id, s.Name, s.SpriteUrl, s.Types.ToList()))
            .ToList();

        return View(new PokemonListViewModel(cards, totalCount, Math.Max(1, page), PageSize, search?.Trim()));
    }

    /// <summary>
    /// Renders the detail page for a single Pokémon identified by name or ID.
    /// Returns 404 if the Pokémon does not exist in the PokéAPI.
    /// </summary>
    /// <param name="name">Pokémon name or numeric ID (case-insensitive).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<IActionResult> Detail(string name, CancellationToken ct = default)
    {
        var detail = await pokemonService.GetDetailAsync(name, ct);
        if (detail is null)
            return NotFound();

        return View(new PokemonDetailViewModel(
            detail.Id,
            detail.Name,
            detail.Height,
            detail.Weight,
            detail.BaseExperience,
            detail.SpriteUrl,
            detail.SpriteShinyUrl,
            detail.Types.ToList(),
            detail.Stats.ToList(),
            detail.Abilities.ToList()));
    }
}
