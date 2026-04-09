using System.Text.Json;

namespace MiniPokedex.Infrastructure.PokeApi.Models;

// ── Shared primitives ────────────────────────────────────────────────────────

/// <summary>A named resource reference as returned by the PokéAPI (name + URL).</summary>
/// <param name="Name">Resource identifier (e.g. "bulbasaur").</param>
/// <param name="Url">Full URL to the resource endpoint.</param>
public record NamedAPIResource(string Name, string Url);

/// <summary>An anonymous resource reference containing only its URL.</summary>
/// <param name="Url">Full URL to the resource endpoint.</param>
public record APIResource(string Url);

/// <summary>Paginated response for endpoints that return named resources.</summary>
/// <param name="Count">Total number of resources available.</param>
/// <param name="Next">URL of the next page, or <c>null</c> if on the last page.</param>
/// <param name="Previous">URL of the previous page, or <c>null</c> if on the first page.</param>
/// <param name="Results">Resources in the current page.</param>
public record NamedAPIResourceList(
    int Count,
    string? Next,
    string? Previous,
    List<NamedAPIResource> Results);

/// <summary>Paginated response for endpoints that return anonymous resources.</summary>
/// <param name="Count">Total number of resources available.</param>
/// <param name="Next">URL of the next page, or <c>null</c> if on the last page.</param>
/// <param name="Previous">URL of the previous page, or <c>null</c> if on the first page.</param>
/// <param name="Results">Resources in the current page.</param>
public record APIResourceList(
    int Count,
    string? Next,
    string? Previous,
    List<APIResource> Results);

// ── Pokémon ──────────────────────────────────────────────────────────────────

/// <summary>A Pokémon's ability slot, indicating whether it is hidden.</summary>
/// <param name="IsHidden">Whether this is a hidden ability.</param>
/// <param name="Slot">Slot number (1 = primary, 2 = secondary, 3 = hidden).</param>
/// <param name="Ability">Reference to the ability resource.</param>
public record PokemonAbility(bool IsHidden, int Slot, NamedAPIResource Ability);

/// <summary>A Pokémon's base stat value and EV yield for a single stat.</summary>
/// <param name="BaseStat">Base stat value.</param>
/// <param name="Effort">EV yield when this Pokémon is defeated.</param>
/// <param name="Stat">Reference to the stat resource.</param>
public record PokemonStat(int BaseStat, int Effort, NamedAPIResource Stat);

/// <summary>A Pokémon's type assignment in a specific slot.</summary>
/// <param name="Slot">Slot number (1 = primary type, 2 = secondary type).</param>
/// <param name="Type">Reference to the type resource.</param>
public record PokemonTypeSlot(int Slot, NamedAPIResource Type);

/// <summary>
/// Full Pokémon data as returned by <c>GET /api/v2/pokemon/{id or name}/</c>.
/// Sprites are kept as a raw <see cref="JsonElement"/> because the sprites object
/// has a deeply nested and irregular shape that does not map cleanly to records.
/// </summary>
/// <param name="Id">National Pokédex number.</param>
/// <param name="Name">Pokémon name in lowercase English.</param>
/// <param name="BaseExperience">Base XP granted when this Pokémon is defeated.</param>
/// <param name="Height">Height in decimetres.</param>
/// <param name="IsDefault">Whether this is the default form of the species.</param>
/// <param name="Order">Sort order across all Pokémon (includes forms).</param>
/// <param name="Weight">Weight in hectograms.</param>
/// <param name="Abilities">All ability slots.</param>
/// <param name="Forms">Named references to the Pokémon's forms.</param>
/// <param name="Species">Reference to the species resource.</param>
/// <param name="Sprites">Raw JSON object containing sprite URLs.</param>
/// <param name="Stats">All base stats with EV yields.</param>
/// <param name="Types">Type slots, ordered by slot number.</param>
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

/// <summary>A localised effect description for an ability.</summary>
/// <param name="Effect">Full effect text.</param>
/// <param name="ShortEffect">Abbreviated effect text for compact displays.</param>
/// <param name="Language">Language of this entry.</param>
public record AbilityEffectEntry(string Effect, string ShortEffect, NamedAPIResource Language);

/// <summary>A Pokémon ability with its localised effect descriptions.</summary>
/// <param name="Id">Ability ID.</param>
/// <param name="Name">Ability name in lowercase English.</param>
/// <param name="EffectEntries">Effect descriptions in various languages.</param>
public record Ability(int Id, string Name, List<AbilityEffectEntry> EffectEntries);

// ── Type ─────────────────────────────────────────────────────────────────────

/// <summary>A Pokémon that has a specific type, with its slot number.</summary>
/// <param name="Slot">Slot number indicating whether this is the primary or secondary type.</param>
/// <param name="Pokemon">Reference to the Pokémon resource.</param>
public record TypePokemonSlot(int Slot, NamedAPIResource Pokemon);

/// <summary>
/// A Pokémon type (e.g. Fire, Water) with damage relations and associated Pokémon.
/// Damage relations are kept as a raw <see cref="JsonElement"/> due to their nested structure.
/// </summary>
/// <param name="Id">Type ID.</param>
/// <param name="Name">Type name in lowercase English.</param>
/// <param name="DamageRelations">Raw JSON object describing offensive and defensive matchups.</param>
/// <param name="Pokemon">All Pokémon that have this type.</param>
public record PokeType(int Id, string Name, JsonElement DamageRelations, List<TypePokemonSlot> Pokemon);

// ── Move ─────────────────────────────────────────────────────────────────────

/// <summary>A Pokémon move with its core battle properties.</summary>
/// <param name="Id">Move ID.</param>
/// <param name="Name">Move name in lowercase English.</param>
/// <param name="Accuracy">Accuracy percentage (0–100), or <c>null</c> if it never misses.</param>
/// <param name="Pp">Power Points (uses), or <c>null</c> if not applicable.</param>
/// <param name="Power">Base power, or <c>null</c> for status moves.</param>
/// <param name="Type">Reference to the type resource.</param>
/// <param name="DamageClass">Reference to the damage class (physical, special, status).</param>
public record Move(
    int Id,
    string Name,
    int? Accuracy,
    int? Pp,
    int? Power,
    NamedAPIResource Type,
    NamedAPIResource DamageClass);

// ── Item ─────────────────────────────────────────────────────────────────────

/// <summary>A holdable or usable in-game item.</summary>
/// <param name="Id">Item ID.</param>
/// <param name="Name">Item name in lowercase English.</param>
/// <param name="Cost">Purchase price in PokéDollars, or <c>null</c> if not purchasable.</param>
/// <param name="Attributes">Item attribute flags (e.g. "holdable", "consumable").</param>
/// <param name="Category">Category the item belongs to.</param>
public record Item(int Id, string Name, int? Cost, List<NamedAPIResource> Attributes, NamedAPIResource Category);

// ── Berry ────────────────────────────────────────────────────────────────────

/// <summary>A Berry that can be grown, held, or used in battle.</summary>
/// <param name="Id">Berry ID.</param>
/// <param name="Name">Berry name in lowercase English.</param>
/// <param name="GrowthTime">Hours to grow one stage, or <c>null</c> if unavailable.</param>
/// <param name="MaxHarvest">Maximum number of berries per harvest, or <c>null</c> if unavailable.</param>
/// <param name="Firmness">Firmness category of the berry.</param>
/// <param name="Item">Reference to the corresponding item resource.</param>
public record Berry(
    int Id,
    string Name,
    int? GrowthTime,
    int? MaxHarvest,
    NamedAPIResource Firmness,
    NamedAPIResource Item);

// ── Evolution Chain ──────────────────────────────────────────────────────────

/// <summary>
/// An evolution chain linking base species to its evolutions.
/// The chain tree is kept as a raw <see cref="JsonElement"/> due to its recursive structure.
/// </summary>
/// <param name="Id">Evolution chain ID.</param>
/// <param name="Chain">Raw JSON object representing the root node of the chain tree.</param>
public record EvolutionChain(int Id, JsonElement Chain);

// ── Machine ──────────────────────────────────────────────────────────────────

/// <summary>A TM or HM that teaches a specific move.</summary>
/// <param name="Id">Machine ID.</param>
/// <param name="Item">Reference to the TM/HM item.</param>
/// <param name="Move">Reference to the move taught by this machine.</param>
/// <param name="VersionGroup">Game version group in which this machine exists.</param>
public record Machine(int Id, NamedAPIResource Item, NamedAPIResource Move, NamedAPIResource VersionGroup);

// ── Contest Type ─────────────────────────────────────────────────────────────

/// <summary>A Pokémon Contest type (e.g. Cool, Cute, Smart).</summary>
/// <param name="Id">Contest type ID.</param>
/// <param name="Name">Contest type name in lowercase English.</param>
/// <param name="BerryFlavor">Berry flavor associated with this contest type.</param>
public record ContestType(int Id, string Name, NamedAPIResource BerryFlavor);

// ── Location ─────────────────────────────────────────────────────────────────

/// <summary>A real-world location within the Pokémon game world.</summary>
/// <param name="Id">Location ID.</param>
/// <param name="Name">Location name in lowercase English.</param>
/// <param name="Region">Region this location belongs to.</param>
public record Location(int Id, string Name, NamedAPIResource Region);

// ── Pokémon Species ──────────────────────────────────────────────────────────

/// <summary>
/// A Pokémon species grouping all forms of the same Pokémon (e.g. all Pikachu forms).
/// </summary>
/// <param name="Id">Species ID (matches the base form's Pokémon ID).</param>
/// <param name="Name">Species name in lowercase English.</param>
/// <param name="Color">Pokédex colour category.</param>
/// <param name="Shape">Body shape category.</param>
/// <param name="EvolutionChain">Reference to the species' evolution chain.</param>
public record PokemonSpecies(
    int Id,
    string Name,
    NamedAPIResource Color,
    NamedAPIResource Shape,
    APIResource EvolutionChain);

// ── Language ─────────────────────────────────────────────────────────────────

/// <summary>A human language used for localised content in the PokéAPI.</summary>
/// <param name="Id">Language ID.</param>
/// <param name="Name">Language name in lowercase English.</param>
/// <param name="Iso639">ISO 639-1 language code, or <c>null</c> if unavailable.</param>
/// <param name="Iso3166">ISO 3166-1 alpha-2 country code, or <c>null</c> if unavailable.</param>
public record Language(int Id, string Name, string? Iso639, string? Iso3166);
