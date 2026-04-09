namespace minipokedex.Models;

/// <summary>
/// View model for the Pokémon detail page, containing all data needed to render
/// the full profile: sprites, types, base stats, and abilities.
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
public record PokemonDetailViewModel(
    int Id,
    string Name,
    int? Height,
    int? Weight,
    int? BaseExperience,
    string? SpriteUrl,
    string? SpriteShinyUrl,
    List<string> Types,
    List<(string Name, int BaseStat)> Stats,
    List<(string Name, bool IsHidden)> Abilities);
