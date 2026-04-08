using System.Text.Json;

namespace MiniPokedex.Infrastructure.PokeApi.Models;

// ── Shared primitives ────────────────────────────────────────────────────────

public record NamedAPIResource(string Name, string Url);

public record APIResource(string Url);

public record NamedAPIResourceList(
    int Count,
    string? Next,
    string? Previous,
    List<NamedAPIResource> Results);

public record APIResourceList(
    int Count,
    string? Next,
    string? Previous,
    List<APIResource> Results);

// ── Pokémon ──────────────────────────────────────────────────────────────────

public record PokemonAbility(bool IsHidden, int Slot, NamedAPIResource Ability);

public record PokemonStat(int BaseStat, int Effort, NamedAPIResource Stat);

public record PokemonTypeSlot(int Slot, NamedAPIResource Type);

public record Pokemon(
    int Id,
    string Name,
    int? BaseExperience,
    int? Height,
    bool IsDefault,
    int? Order,
    int? Weight,
    List<PokemonAbility> Abilities,
    List<NamedAPIResource> Forms,
    NamedAPIResource Species,
    JsonElement Sprites,
    List<PokemonStat> Stats,
    List<PokemonTypeSlot> Types);

// ── Ability ──────────────────────────────────────────────────────────────────

public record AbilityEffectEntry(string Effect, string ShortEffect, NamedAPIResource Language);

public record Ability(int Id, string Name, List<AbilityEffectEntry> EffectEntries);

// ── Type ─────────────────────────────────────────────────────────────────────

public record TypePokemonSlot(int Slot, NamedAPIResource Pokemon);

public record PokeType(int Id, string Name, JsonElement DamageRelations, List<TypePokemonSlot> Pokemon);

// ── Move ─────────────────────────────────────────────────────────────────────

public record Move(
    int Id,
    string Name,
    int? Accuracy,
    int? Pp,
    int? Power,
    NamedAPIResource Type,
    NamedAPIResource DamageClass);

// ── Item ─────────────────────────────────────────────────────────────────────

public record Item(int Id, string Name, int? Cost, List<NamedAPIResource> Attributes, NamedAPIResource Category);

// ── Berry ────────────────────────────────────────────────────────────────────

public record Berry(
    int Id,
    string Name,
    int? GrowthTime,
    int? MaxHarvest,
    NamedAPIResource Firmness,
    NamedAPIResource Item);

// ── Evolution Chain ──────────────────────────────────────────────────────────

public record EvolutionChain(int Id, JsonElement Chain);

// ── Machine ──────────────────────────────────────────────────────────────────

public record Machine(int Id, NamedAPIResource Item, NamedAPIResource Move, NamedAPIResource VersionGroup);

// ── Contest Type ─────────────────────────────────────────────────────────────

public record ContestType(int Id, string Name, NamedAPIResource BerryFlavor);

// ── Location ─────────────────────────────────────────────────────────────────

public record Location(int Id, string Name, NamedAPIResource Region);

// ── Pokémon Species ──────────────────────────────────────────────────────────

public record PokemonSpecies(
    int Id,
    string Name,
    NamedAPIResource Color,
    NamedAPIResource Shape,
    APIResource EvolutionChain);

// ── Language ─────────────────────────────────────────────────────────────────

public record Language(int Id, string Name, string? Iso639, string? Iso3166);
