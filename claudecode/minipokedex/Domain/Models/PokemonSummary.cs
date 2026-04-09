namespace MiniPokedex.Domain.Models;

/// <summary>
/// Lightweight read model for a Pokémon entry shown in list and search results.
/// Lives in the Domain layer so both Application and Infrastructure can reference it
/// without any dependency on Presentation or external API shapes.
/// </summary>
/// <param name="Id">National Pokédex number.</param>
/// <param name="Name">Pokémon name in lowercase English.</param>
/// <param name="SpriteUrl">Front-facing sprite URL, or <c>null</c> if unavailable.</param>
/// <param name="Types">Ordered list of type names (e.g. "fire", "flying").</param>
public record PokemonSummary(
    int Id,
    string Name,
    string? SpriteUrl,
    IReadOnlyList<string> Types);
