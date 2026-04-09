using MiniPokedex.Domain.Models;
using MiniPokedex.Domain.Ports;

namespace MiniPokedex.Application.Services;

/// <summary>
/// Application service for the Pokédex feature.
/// Orchestrates use cases by delegating to <see cref="IPokemonRepository"/>.
/// Controllers depend on this service rather than on the repository or Infrastructure directly,
/// keeping the dependency graph aligned with Clean Architecture.
/// </summary>
public sealed class PokemonAppService(IPokemonRepository repository)
{
    /// <summary>
    /// Returns a paginated page of Pokémon summaries, applying a search filter when provided.
    /// Normalises the page number and search term before delegating to the repository.
    /// </summary>
    /// <param name="page">1-based page number (values below 1 are clamped to 1).</param>
    /// <param name="pageSize">Maximum number of items per page.</param>
    /// <param name="search">Optional search term; <c>null</c> or whitespace means no filter.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The page items and the total count of matching Pokémon.</returns>
    public Task<(IReadOnlyList<PokemonSummary> Items, int TotalCount)> GetPageAsync(
        int page, int pageSize, string? search, CancellationToken ct)
    {
        page = Math.Max(1, page);

        return string.IsNullOrWhiteSpace(search)
            ? repository.ListPageAsync(page, pageSize, ct)
            : repository.SearchPageAsync(search.Trim().ToLowerInvariant(), page, pageSize, ct);
    }

    /// <summary>
    /// Returns the full detail of a single Pokémon, or <c>null</c> if not found.
    /// </summary>
    /// <param name="idOrName">Pokémon name or numeric ID (normalised to lowercase before querying).</param>
    /// <param name="ct">Cancellation token.</param>
    public Task<PokemonDetail?> GetDetailAsync(string idOrName, CancellationToken ct)
        => repository.GetDetailAsync(idOrName.ToLowerInvariant(), ct);
}
