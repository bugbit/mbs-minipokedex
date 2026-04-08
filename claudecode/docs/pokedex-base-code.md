# Pokédex Base Code

## Summary

Core Pokédex functionality: paginated list of Pokémon with sprites and type badges, and a full detail view with stats, abilities, and shiny sprite toggle. Data comes from PokéAPI via `IPokeApiClient`.

## Files

| File | Type | Purpose |
|------|------|---------|
| `Controllers/PokemonController.cs` | New | List + detail actions |
| `Models/PokemonListViewModel.cs` | New | `PokemonListViewModel`, `PokemonCardViewModel` |
| `Models/PokemonDetailViewModel.cs` | New | `PokemonDetailViewModel` |
| `Views/Pokemon/Index.cshtml` | New | Card grid with search bar and pagination |
| `Views/Pokemon/Detail.cshtml` | New | Full detail: sprite, stats, types, abilities |
| `wwwroot/css/site.css` | Modified | Type badge colors, card and detail styles |
| `Views/Shared/_Layout.cshtml` | Modified | Brand updated; Pokédex nav link added |

## Controller

### `Index(int page, string? search)`

- If `search` is non-empty → `RedirectToAction("Detail", new { name })`.
- Otherwise: fetches the paginated list with `ListPokemonAsync(20, offset)`, then fetches all 20 Pokémon **in parallel** via `Task.WhenAll` to get sprites and types in a single round-trip.
- Maps results to `PokemonCardViewModel` (id, name, sprite URL, types), sorted by id.

### `Detail(string name)`

- Calls `GetPokemonAsync(name)`. Returns `404` if not found.
- Extracts sprite URL from `Pokemon.Sprites` (`JsonElement`): tries `other.official-artwork.front_default` first, falls back to `front_default`.
- Maps stats and abilities to typed tuples for the view.

## ViewModels

```csharp
// List
record PokemonCardViewModel(int Id, string Name, string? SpriteUrl, List<string> Types);
record PokemonListViewModel(List<PokemonCardViewModel> Pokemon, int TotalCount, int Page, int PageSize)
// Computed: TotalPages, HasPrevious, HasNext

// Detail
record PokemonDetailViewModel(
    int Id, string Name, int? Height, int? Weight, int? BaseExperience,
    string? SpriteUrl, string? SpriteShinyUrl,
    List<string> Types,
    List<(string Name, int BaseStat)> Stats,
    List<(string Name, bool IsHidden)> Abilities);
```

## Views

### `Index.cshtml`

- Search form → redirects to Detail on submit.
- Bootstrap responsive grid: 2 cols (xs) → 3 (sm) → 4 (md) → 5 (lg).
- Each card: sprite (96×96), `#001` number, name, type badges.
- Pagination: shows a window of ±2 pages around the current page.

### `Detail.cshtml`

- Two-column layout (stacks on mobile).
- **Left column:** official artwork (220×220), type badges, height/weight/base XP table, abilities.
- **Right column:** base stats as labeled Bootstrap progress bars; total at the bottom.
- `✨ Toggle Shiny` button swaps `img.src` between normal and shiny sprites via inline JS.
- Stat bar color: red (<50), yellow (<80), green (≥80).

## CSS — Type badges

18 type classes follow the canonical Pokémon color palette:

```css
.type-fire    { background-color: #F08030; }
.type-water   { background-color: #6890F0; }
/* … etc. */
```

Used as `.type-badge.type-{typename}` — applied dynamically from the type name string.

## Sprite URL strategy

For list cards, sprites are obtained by calling `GetPokemonAsync` for each entry (parallel). The controller tries:

1. `sprites.other["official-artwork"].front_default` — high-resolution artwork
2. `sprites.front_default` — pixel sprite fallback

## Design decisions

- **N+1 on the list page is intentional** — 20 parallel requests is fast enough for a demo/learning project. A production version would use a cache layer or a bulk endpoint.
- **Search = exact name redirect** — PokéAPI does not support partial-name search; redirecting to Detail on exact match is the cleanest solution.
- **`JsonElement` for sprites** — the sprites object in PokéAPI is deeply nested and inconsistent across generations; using `JsonElement` avoids a brittle nested record structure.
