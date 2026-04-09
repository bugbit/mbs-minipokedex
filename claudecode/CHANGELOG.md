# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.4.0] - 2026-04-09

### Added

- `Domain/Models/PokemonSummary.cs` — read model ligero para listas y búsquedas.
- `Domain/Models/PokemonDetail.cs` — read model completo para la página de detalle.
- `Domain/Ports/IPokemonRepository.cs` — puerto (interfaz) de repositorio declarado en Domain; expone `ListPageAsync`, `SearchPageAsync` y `GetDetailAsync`.
- `Application/Services/PokemonAppService.cs` — servicio de aplicación que orquesta los casos de uso (paginación, búsqueda, detalle) delegando en `IPokemonRepository`.
- `Infrastructure/PokeApi/PokeApiPokemonRepository.cs` — implementación concreta de `IPokemonRepository` que usa `IPokeApiClient`; contiene todo el mapeo de modelos API → modelos de dominio.
- `docs/clean-architecture-refactor.md` — documentación del rediseño arquitectónico.

### Changed

- `PokemonController` refactorizado a controlador delgado: elimina lógica de búsqueda, paginación y mapeo; solo llama a `PokemonAppService` y proyecta resultados a ViewModels.
- `Program.cs` actualizado: registra `IPokemonRepository → PokeApiPokemonRepository` y `PokemonAppService` en el contenedor de DI.
- `CLAUDE.md` actualizado con sección `## Design Principles` describiendo Clean Architecture, DDD y SOLID.

## [0.3.0] - 2026-04-08

### Added

- `PokemonController` with `Index` (paginated list + search redirect) and `Detail` actions.
- `PokemonListViewModel` and `PokemonCardViewModel` for the list view.
- `PokemonDetailViewModel` for the detail view (stats, types, abilities, sprites).
- `Views/Pokemon/Index.cshtml` — Pokémon card grid with pagination.
- `Views/Pokemon/Detail.cshtml` — full detail page: official artwork, type badges, base stat bars, abilities, height/weight/base XP, shiny toggle.
- Pokémon type badge CSS classes (18 types with canonical colors) in `site.css`.
- Pokémon card and detail sprite styles in `site.css`.
- "Pokédex" link in the navbar; brand updated to "MiniPokédex".

### Changed

- `_Layout.cshtml` navbar brand and links updated.

## [0.2.0] - 2026-04-08

### Added

- Typed HTTP client for PokéAPI v2 (`IPokeApiClient` / `PokeApiClient`) with 24 methods covering all endpoints defined in `context/pokeapi-openapi.json`.
- C# record models for all PokéAPI schemas (`Pokemon`, `Ability`, `PokeType`, `Move`, `Item`, `Berry`, `EvolutionChain`, `Machine`, `ContestType`, `Location`, `PokemonSpecies`, `Language`, and shared primitives).
- `<Version>` field in `minipokedex.csproj` to track project version.
- `docs/pokeapi-http-client.md` documenting the HTTP client design and usage.
- `CHANGELOG.md` (this file) following Keep a Changelog format.
- Versioning and documentation rules in `CLAUDE.md`.

## [0.1.0] - 2026-04-08

### Added

- Initial ASP.NET Core MVC scaffold targeting .NET 10.0.
- `Program.cs` with MVC services, HTTPS redirect, routing, static assets, and default controller route.
- `HomeController` with `Index` and `Privacy` actions.
- `ErrorViewModel` and error view.
- Bootstrap 5 + jQuery layout (`Views/Shared/_Layout.cshtml`).
- Visual Studio solution file (`minipokedex.sln`).
- `CLAUDE.md` with project guidance for Claude Code.
