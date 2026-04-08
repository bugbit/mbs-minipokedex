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

## Key Notes

- Target framework: `net10.0` with nullable reference types and implicit usings enabled.
- No external NuGet dependencies beyond the SDK defaults yet — the Pokédex feature set is to be built on top of this scaffold.
- The PokéAPI (`https://pokeapi.co/api/v2/`) is the natural data source for Pokémon data; no API key required.
