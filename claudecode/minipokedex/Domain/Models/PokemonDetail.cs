namespace MiniPokedex.Domain.Models;

/// <summary>
/// Full read model for the Pokémon detail page.
/// Lives in the Domain layer so all layers can use it without coupling to
/// the PokéAPI response shape or any Presentation concern.
/// </summary>
/// <param name="Id">National Pokédex number.</param>
/// <param name="Name">Pokémon name in lowercase English.</param>
/// <param name="Height">Height in decimetres, or <c>null</c> if unavailable.</param>
/// <param name="Weight">Weight in hectograms, or <c>null</c> if unavailable.</param>
/// <param name="BaseExperience">Base XP yield when defeated, or <c>null</c> if unavailable.</param>
/// <param name="SpriteUrl">URL of the front official artwork sprite.</param>
/// <param name="SpriteShinyUrl">URL of the shiny variant of the official artwork sprite.</param>
/// <param name="Types">Ordered list of type names (e.g. "water", "psychic").</param>
/// <param name="Stats">Base stats as (display label, value) pairs in PokéAPI order.</param>
/// <param name="Abilities">Abilities as (name, isHidden) pairs ordered by slot.</param>
public record PokemonDetail(
    int Id,
    string Name,
    int? Height,
    int? Weight,
    int? BaseExperience,
    string? SpriteUrl,
    string? SpriteShinyUrl,
    IReadOnlyList<string> Types,
    IReadOnlyList<(string Label, int Value)> Stats,
    IReadOnlyList<(string Name, bool IsHidden)> Abilities);
