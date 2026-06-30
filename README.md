# CountryExplorer

Aplicación web para explorar países del mundo. Permite buscar países, guardarlos, marcarlos como favoritos y archivarlos.

## Tecnologías

- ASP.NET Core 8 (Minimal API + MVC)
- Entity Framework Core con SQL Server
- REST Countries API (fuente de datos externa)

## Estructura

```
ExplorerApp/           → Minimal API (backend)
ExplorerApp.WebMVC/    → Frontend MVC
ExplorerApp.Services/  → Lógica de negocio
ExplorerApp.Infrastructure/ → Modelos, BD, migraciones
ExplorerApp.Shared/    → DTOs compartidos
ExplorerApp.Tests/     → Pruebas unitarias
```

## Funcionalidades

- Buscar países por nombre
- Guardar países en la base de datos local
- Archivar / desarchivar países
- Agregar y eliminar favoritos
- Dashboard con estadísticas y actividad reciente

## Cómo correr el proyecto

1. Configurar la cadena de conexión en `appsettings.json`
2. Las migraciones se aplican automáticamente al iniciar
3. Correr el proyecto `ExplorerApp` (API) y `ExplorerApp.WebMVC` (frontend)

```bash
dotnet run --project ExplorerApp
dotnet run --project ExplorerApp.WebMVC
```

La API queda disponible en `https://localhost:{puerto}/swagger`
