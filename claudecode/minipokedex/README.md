### Instrucciones

dotnet new mvc -n minipokedex

##claude code:

/init
/plugin marketplace add Aaronontheweb/dotnet-skills
/plugin install dotnet-skills

en context hay pokeapi-openapi.json
* Hay un spec OpenAPI de PokéAPI en context/pokeapi-openapi.json. ¿Qué quieres hacer con él? Por ejemplo:
  - Usarlo como referencia para generar un cliente HTTP tipado en C# para el proyecto
  - Explorar qué endpoints/modelos están definidos
  - Otra cosa

Usarlo como referencia para generar un cliente HTTP tipado en C# para el proyecto

Todo el trabajo realizado por el asistente, incluyendo código y explicaciones, debe ser documentado en archivos de Markdown dentro de la carpeta `/docs`. Cada nueva funcionalidad o cambio significativo debe tener su propio archivo de documentación.

añadelo en CLAUDE.md

añade en CLAUDE.md: Cada vez que se realice un cambio o se añada una nueva funcionalidad, el asistente debe aumentar la versión del proyecto

añade en CLAUDE.md: Todos los cambios (añadidos, modificados, corregidos, etc.) deben ser registrados en el archivo `CHANGELOG.md` siguiendo la estructura y el formato estricto de [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/).