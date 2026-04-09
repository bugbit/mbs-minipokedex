namespace minipokedex.Models;

/// <summary>
/// Lightweight summary of a single Pokémon shown as a card in the list view.
/// </summary>
/// <param name="Id">National Pokédex number.</param>
/// <param name="Name">Pokémon name in lowercase English.</param>
/// <param name="SpriteUrl">URL of the front-facing sprite, or <c>null</c> if unavailable.</param>
/// <param name="Types">Ordered list of type names (e.g. "fire", "flying").</param>
public record PokemonCardViewModel(
    int Id,
    string Name,
    string? SpriteUrl,
    List<string> Types);

/// <summary>
/// View model for the paginated Pokémon list page.
/// </summary>
/// <param name="Pokemon">Cards to display on the current page.</param>
/// <param name="TotalCount">Total number of Pokémon available across all pages.</param>
/// <param name="Page">Current 1-based page number.</param>
/// <param name="PageSize">Maximum number of cards per page.</param>
/// <param name="Search">Active search term, or <c>null</c> when browsing normally.</param>
public record PokemonListViewModel(
    List<PokemonCardViewModel> Pokemon,
    int TotalCount,
    int Page,
    int PageSize,
    string? Search = null)
{
    /// <summary>Total number of pages, calculated from <see cref="TotalCount"/> and <see cref="PageSize"/>.</summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>Whether a previous page exists.</summary>
    public bool HasPrevious => Page > 1;

    /// <summary>Whether a next page exists.</summary>
    public bool HasNext => Page < TotalPages;

    /// <summary>Whether the list is showing search results instead of the full paginated catalogue.</summary>
    public bool IsSearching => !string.IsNullOrEmpty(Search);
}
