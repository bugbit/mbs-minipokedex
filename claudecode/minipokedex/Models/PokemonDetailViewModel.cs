namespace minipokedex.Models;

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
