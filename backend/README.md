# Ferreteria.Api

API del catálogo público de ferretería: **ASP.NET Core (.NET 10)**, **EF Core + SQLite** y conversión
USD → Bs. con la tasa oficial de **DolarAPI**. No requiere SQL Server ni conexión a internet para
arrancar: si el tercero falla, la tasa se sirve desde caché o desde un valor de respaldo.

## Requisitos

- .NET SDK 10
- Herramienta local `dotnet-ef` (ya versionada en `.config/dotnet-tools.json`)

```bash
dotnet tool restore
```

## Ejecutar

```bash
# desde la raíz del repositorio
dotnet run --project backend/Ferreteria.Api
```

La API escucha en `http://localhost:5148` y aplica las migraciones pendientes al arrancar
(`Ferreteria.db` se crea en la carpeta del proyecto; está ignorada por git).

## Endpoints

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/productos?busqueda=&categoriaId=&page=&pageSize=` | Listado paginado `{ items, total, page, pageSize }`. Búsqueda por nombre/SKU sin distinguir mayúsculas ni tildes. `page ≥ 1`, `pageSize` 1..100 (12 por defecto). |
| GET | `/api/productos/{id}` | Producto con su categoría; `404` en formato ProblemDetails si no existe. |
| GET | `/api/categorias` | `[{ id, nombre, slug, conteo }]` con el número de productos por categoría. |
| GET | `/api/tasa` | `{ usdBs, fechaActualizacion, fuente }` con `fuente` ∈ `dolarapi \| cache \| fallback`. |

Los precios viajan siempre en USD como `decimal` y el JSON se serializa en camelCase.

## Tasa de cambio

`https://ve.dolarapi.com/v1/dolares/oficial` se consulta con `IHttpClientFactory`, caché en memoria de
5 minutos y timeout acotado. Si DolarAPI falla o tarda, se devuelve el último valor conocido
(`fallback`) o, si nunca hubo uno, `Tasa:ValorRespaldo`. El endpoint nunca responde 500 por culpa del
tercero.

Configuración en `appsettings.json`:

```json
{
  "Cors": { "OrigenesPermitidos": ["http://localhost:5173"] },
  "Tasa": {
    "Url": "https://ve.dolarapi.com/v1/dolares/oficial",
    "TimeoutSegundos": 5,
    "CacheMinutos": 5,
    "ValorRespaldo": 100.0
  }
}
```

## Base de datos y migraciones

Datos semilla: 7 categorías y 40 productos insertados por la migración inicial
(`Data/SeedData.cs`, aplicado con `HasData`, por lo que es idempotente).

```bash
dotnet ef migrations add <Nombre> --project backend/Ferreteria.Api --startup-project backend/Ferreteria.Api
dotnet ef database update --project backend/Ferreteria.Api --startup-project backend/Ferreteria.Api
```

## Pruebas

```bash
cd backend
dotnet test
```

Las pruebas de integración usan `WebApplicationFactory` sobre una base SQLite temporal y las de la tasa
sustituyen DolarAPI con un `HttpMessageHandler` simulado: no necesitan red.
