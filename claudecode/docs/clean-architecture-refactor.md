# Refactor: Clean Architecture + DDD + SOLID

**Versión:** 0.4.0  
**Fecha:** 2026-04-09

## Propósito

Reestructurar el proyecto para cumplir con Clean Architecture, DDD y los principios SOLID, eliminando el acoplamiento directo entre el controlador y la infraestructura de la API.

## Problema previo

El `PokemonController` (v0.3.0) violaba varios principios:

| Violación | Descripción |
|---|---|
| **S** | El controller tenía lógica de búsqueda, paginación y mapeo además de la responsabilidad HTTP |
| **I** | Dependía de `IPokeApiClient` con 20 métodos, usando solo 2 |
| **D** | `IPokeApiClient` vivía en `Infrastructure`, el controller importaba modelos de API directamente |
| **Clean Arch** | Faltaban las capas Domain y Application; Presentation conocía Infrastructure |

## Solución: estructura de capas

```
Presentation  (Controllers/)
      │ depende de ▼
Application   (Application/Services/)
      │ depende de ▼
Domain        (Domain/Models/, Domain/Ports/)
      ▲ implementado por
Infrastructure (Infrastructure/PokeApi/)
```

### Regla de dependencias

Las dependencias apuntan **siempre hacia el interior**. Infrastructure implementa interfaces de Domain; Presentation llama a Application; nadie en capas interiores conoce capas exteriores.

## Archivos creados

### Domain

| Archivo | Descripción |
|---|---|
| `Domain/Models/PokemonSummary.cs` | Read model ligero: Id, Name, SpriteUrl, Types |
| `Domain/Models/PokemonDetail.cs` | Read model completo con stats, abilities, sprites |
| `Domain/Ports/IPokemonRepository.cs` | Puerto (interfaz) con 3 métodos: `ListPageAsync`, `SearchPageAsync`, `GetDetailAsync` |

La interfaz del repositorio está en **Domain** (no en Infrastructure), de modo que Application depende de una abstracción pura sin referencias externas.

### Application

| Archivo | Descripción |
|---|---|
| `Application/Services/PokemonAppService.cs` | Orquesta los casos de uso: decide si paginar o buscar, normaliza el término y delega en `IPokemonRepository` |

### Infrastructure

| Archivo | Descripción |
|---|---|
| `Infrastructure/PokeApi/PokeApiPokemonRepository.cs` | Implementa `IPokemonRepository` usando `IPokeApiClient`; contiene `ToSummary`, `ToDetail`, `GetSpriteUrl`, `StatLabel` |

## Archivos modificados

| Archivo | Cambio |
|---|---|
| `Controllers/PokemonController.cs` | Simplificado a controlador delgado: llama a `PokemonAppService` y proyecta al ViewModel |
| `Program.cs` | Registra `IPokemonRepository → PokeApiPokemonRepository` y `PokemonAppService` en DI |

## Principios SOLID aplicados

| Principio | Antes | Después |
|---|---|---|
| **S** | Controller: HTTP + búsqueda + paginación + mapeo | Controller: solo HTTP; AppService: orquestación; Repository: mapeo |
| **I** | Controller depende de `IPokeApiClient` (20 métodos) | Controller depende de `PokemonAppService`; Repository usa `IPokeApiClient` internamente |
| **D** | `IPokeApiClient` en Infrastructure; controller importa modelos API | `IPokemonRepository` en Domain; controller no conoce Infrastructure |

## Cómo usar

```csharp
// Inyección en el controller (Presentation)
public class PokemonController(PokemonAppService pokemonService) : Controller { ... }

// Inyección en el servicio de aplicación (Application → Domain)
public sealed class PokemonAppService(IPokemonRepository repository) { ... }

// Registro en DI (Program.cs)
builder.Services.AddScoped<IPokemonRepository, PokeApiPokemonRepository>();
builder.Services.AddScoped<PokemonAppService>();
```
