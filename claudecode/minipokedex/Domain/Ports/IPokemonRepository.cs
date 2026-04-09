using MiniPokedex.Domain.Models;

namespace MiniPokedex.Domain.Ports;

/// <summary>
/// Read-only repository port for Pokémon data.
/// Declared in the Domain layer so the dependency rule is respected:
/// Application depends on this abstraction; Infrastructure provides the implementation.
/// </summary>
public interface IPokemonRepository
{
    /// <summary>
    /// Returns a single page of Pokémon summaries from the full catalogue.
    /// </summary>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Maximum number of items per page.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The summaries for the requested page and the total catalogue size.</returns>
    Task<(IReadOnlyList<PokemonSummary> Items, int TotalCount)> ListPageAsync(
        int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Searches Pokémon by name fragment or exact Pokédex number and returns a single page.
    /// </summary>
    /// <param name="term">Normalised (lowercase, trimmed) search term.</param>
    /// <param name="page">1-based page number within the search results.</param>
    /// <param name="pageSize">Maximum number of items per page.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The matching summaries for the page and the total number of matches.</returns>
    Task<(IReadOnlyList<PokemonSummary> Items, int TotalCount)> SearchPageAsync(
        string term, int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Returns the full detail of a single Pokémon by name or numeric ID,
    /// or <c>null</c> if no Pokémon with that identifier exists.
    /// </summary>
    /// <param name="idOrName">Pokémon name (lowercase) or numeric ID as a string.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<PokemonDetail?> GetDetailAsync(string idOrName, CancellationToken ct = default);
}
