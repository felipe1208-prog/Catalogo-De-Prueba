# Catálogo de Ferretería

Catálogo público de ferretería con enfoque e-commerce: búsqueda de productos, navegación por
categorías, ficha de producto, carrito de compras y precios en **USD ($)** con conversión en
tiempo real a **Bolívares (Bs.)**.

Monolito con backend ASP.NET Core y frontend Vue 3 servido como SPA. Los precios base se
guardan en USD y la tasa oficial se consulta a [DolarAPI](https://ve.dolarapi.com/v1/dolares/oficial),
con caché en memoria y respaldo local si la API externa no responde.

## Stack

| Capa | Tecnología |
| --- | --- |
| Backend | ASP.NET Core Web API · **.NET 10** (`Ferreteria.Api`) |
| Persistencia | Entity Framework Core + **SQLite** local (`Ferreteria.db`, migraciones) |
| Pruebas backend | **xUnit** (`Ferreteria.Api.Tests`) |
| Frontend | **Vue.js 3** (Composition API) + **Vite** |
| Estilos | **Tailwind CSS** |
| Estado y rutas | **Pinia** + **Vue Router** |
| HTTP | **Axios** |
| Pruebas frontend | **Vitest** |
| CI | GitHub Actions (`.github/workflows/ci.yml`) |

## Estructura del repositorio

| Ruta | Contenido |
| --- | --- |
| `backend/Ferreteria.Api/` | API Web (controllers, servicios, EF Core, migraciones SQLite) |
| `backend/Ferreteria.Api.Tests/` | Pruebas unitarias xUnit del backend |
| `frontend/` | SPA Vue 3 + Vite + Tailwind, con pruebas Vitest |
| `.github/workflows/` | Pipeline de CI que corre en cada Pull Request hacia `main` |

> `backend/` y `frontend/` se crean en sus respectivos Issues de sprint. El pipeline detecta
> automáticamente si existen y omite esas fases mientras no estén (el check queda en verde como
> "nada por hacer").

## Cómo ejecutar el backend

Requisitos: [SDK de .NET 10](https://dotnet.microsoft.com/download).

```bash
cd backend
dotnet restore
dotnet build
dotnet test                    # pruebas xUnit
dotnet run --project Ferreteria.Api
```

La API queda disponible en la URL que imprima la consola (por defecto
`http://localhost:5xxx`); la base SQLite se genera al aplicar migraciones:

```bash
dotnet ef database update --project Ferreteria.Api
```

## Cómo ejecutar el frontend

Requisitos: **Node 22+** y npm.

```bash
cd frontend
npm ci            # si existe package-lock.json (si no: npm install)
npm run dev       # servidor de desarrollo con Vite
npm test -- --run # pruebas unitarias con Vitest (modo no interactivo)
npm run build     # build de producción en frontend/dist
```

## CI en local

El pipeline (`.github/workflows/ci.yml`) se dispara en `pull_request` hacia `main` y de forma
manual con `workflow_dispatch`. Usa `permissions: contents: read` y cancela ejecuciones viejas
de la misma rama (`concurrency`).

- **Job `backend`**: `dotnet restore`, `dotnet build --configuration Release` y
  `dotnet test` sobre el `.sln` del backend o sobre `Ferreteria.Api.Tests`.
- **Job `frontend`**: `npm ci` (o `npm install` sin lock), `npm test -- --run` y
  `npm run build`.
- Los tests **no** usan `continue-on-error`: una prueba roja rompe el check.

Para reproducir el CI antes de abrir el PR, ejecuta los mismos comandos:

```bash
# Backend
dotnet restore backend && dotnet build backend --configuration Release && dotnet test backend --configuration Release

# Frontend
cd frontend && npm ci && npm test -- --run && npm run build
```

Opcional, con [act](https://github.com/nektos/act) para simular el runner de GitHub:

```bash
act pull_request -W .github/workflows/ci.yml
```

> `act` no está instalado por defecto; los comandos de arriba son la forma soportada de validar
> el pipeline en local.

## Flujo de trabajo

1. Cada tarea nace de un Issue en GitHub con contexto y criterios de aceptación.
2. Se trabaja en una rama `<tipo>/<id-issue>-<descripcion-corta>` (nunca sobre `main`).
3. Commits atómicos en [Conventional Commits](https://www.conventionalcommits.org/es/)
   (`tipo(alcance): descripción` imperativa, máx. 50 caracteres).
4. Se abre PR hacia `main` con `Closes #<id>`; el CI debe estar en verde y @qa audita antes
   de fusionar con squash.

Detalles completos en [`ENGINEERING_GUIDE.md`](./ENGINEERING_GUIDE.md).
