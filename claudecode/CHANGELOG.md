# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
