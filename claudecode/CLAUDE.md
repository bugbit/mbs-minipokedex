# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Mini Pokédex — an ASP.NET Core MVC application targeting .NET 10.0, built incrementally with Claude Code. The project lives under `claudecode/minipokedex/`.

## Commands

All commands run from `claudecode/minipokedex/`:

```bash
# Run the app
dotnet run

# Build
dotnet build

# Run tests (if a test project exists)
dotnet test

# Watch mode (auto-restart on file changes)
dotnet watch run
```

## Architecture

Standard ASP.NET Core MVC layout under `claudecode/minipokedex/`:

- **Program.cs** — app bootstrap: registers MVC services, configures middleware pipeline (HTTPS redirect, routing, authorization, static assets), maps the default `{controller=Home}/{action=Index}/{id?}` route.
- **Controllers/** — MVC controllers. Currently only `HomeController`.
- **Models/** — view models and domain models. Currently only `ErrorViewModel`.
- **Views/** — Razor views, organized by controller name. Shared layout in `Views/Shared/_Layout.cshtml` uses Bootstrap 5 + jQuery.
- **wwwroot/** — static assets. Client libs (Bootstrap, jQuery, jQuery Validation) are vendored under `wwwroot/lib/`.

## Changelog

Todos los cambios deben registrarse en `claudecode/CHANGELOG.md` siguiendo estrictamente el formato de [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/). Categorías válidas: `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, `Security`. Cada entrada va bajo la versión correspondiente con su fecha.

## Versioning

After every change or new feature, increment the `<Version>` field in `claudecode/minipokedex/minipokedex.csproj` following SemVer:

- **Patch** (`0.0.x`) — bug fixes, minor corrections.
- **Minor** (`0.x.0`) — new features, backwards-compatible additions.
- **Major** (`x.0.0`) — breaking changes or major redesigns.

Current version: `0.3.0` (Pokédex base code: list, detail, styles).

## Documentation

Every new feature or significant change must be documented in a Markdown file under `claudecode/docs/`. Create the `docs/` folder if it doesn't exist yet. Each file should cover: purpose, files created/modified, key design decisions, and a usage example.

## Idioma

Todo el texto visible en las páginas (etiquetas, botones, mensajes, textos de navegación, etc.) debe estar en **castellano**. El atributo `lang` del HTML debe ser `es`.

## Code Quality

Todo el código debe estar limpio y bien documentado:

- Clases, métodos y propiedades públicos deben incluir comentarios XML (`/// <summary>`).
- Evitar código muerto, variables sin usar y lógica innecesariamente compleja.
- Cada método debe tener una única responsabilidad clara.
- Los nombres de variables, métodos y clases deben ser descriptivos y coherentes con el resto del proyecto.

## Key Notes

- Target framework: `net10.0` with nullable reference types and implicit usings enabled.
- No external NuGet dependencies beyond the SDK defaults yet — the Pokédex feature set is to be built on top of this scaffold.
- The PokéAPI (`https://pokeapi.co/api/v2/`) is the natural data source for Pokémon data; no API key required.
