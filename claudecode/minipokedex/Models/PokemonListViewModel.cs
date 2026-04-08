namespace minipokedex.Models;

public record PokemonCardViewModel(
    int Id,
    string Name,
    string? SpriteUrl,
    List<string> Types);

public record PokemonListViewModel(
    List<PokemonCardViewModel> Pokemon,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}
