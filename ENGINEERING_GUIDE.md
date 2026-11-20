# Guía de Ingeniería y Control de Versiones

## 1. Arquitectura y Stack del Monolito
- **Backend (ackend/):** ASP.NET Core Web API en **.NET 10** (Ferreteria.Api) + Entity Framework Core con **SQLite** local (erreteria.db mediante migraciones) + pruebas unitarias en **xUnit** (Ferreteria.Api.Tests). Prohibido conectarse a SQL Server externo.
- **Frontend (rontend/):** **Vue.js 3** (Composition API) + Vite + Tailwind CSS + Pinia + Vue Router + Axios + pruebas unitarias con **Vitest**.
- **Dominio y Moneda:** Catálogo público de ferretería estilo Amazon (búsqueda, categorías, ficha de producto y carrito en Pinia). Precios base en **USD ($)** con conversión en tiempo real a **Bolívares (Bs.)** consumiendo **DolarAPI** (https://ve.dolarapi.com/v1/dolares/oficial con caché en memoria y respaldo si la API externa falla).
- **CI/CD (.github/workflows/ci.yml):** Pipeline obligatorio en cada Pull Request hacia main que ejecute dotnet test (backend) y 
pm test + 
pm run build (frontend).

## 2. Reglas de UX / UI (Frontend)
- Diseño limpio, comercial y orientado a conversión estilo e-commerce moderno.
- **Estricto:** Prohibido exagerar con animaciones, prohibidos glows, neones o bordes neón, prohibido usar ont-mono en la interfaz y prohibido mostrar tecnicismos al usuario final.

## 3. Gestión de Issues (La Fuente de la Verdad)
- **Regla absoluta:** Ninguna línea de código se escribe sin un Issue previo en GitHub (gh issue create).
- **Títulos estructurados:** Feature: ..., Bug: ..., Refactor: ..., Chore: ..., Docs: ....
- **Cuerpo obligatorio de todo Issue:**
  - ## Contexto: Por qué es necesaria la tarea.
  - ## Criterios de Aceptación: Checklist binario - [ ] verificable.

## 4. Gestión de Ramas (Branching)
- **Regla de Oro Inquebrantable:** Jamás se ejecuta git commit ni git push directo sobre main.
- **Nomenclatura obligatoria:** <tipo>/<numero-de-issue>-<descripcion-corta>
  - Tipos válidos: eat/, ugfix/, hotfix/, chore/, efactor/, docs/.
  - Ejemplo: eat/1-estructura-backend-sqlite
- **Ciclo de vida:**
  1. git switch main && git pull origin main
  2. git switch -c feat/<id>-<slug>
  3. Commits atómicos
  4. git push -u origin feat/<id>-<slug>

## 5. Estándares de Commits (Conventional Commits)
- **Fórmula:** 	ipo(alcance): descripción en modo imperativo (máx. 50 caracteres).
- **Tipos:** eat, ix, efactor, chore, style, docs, 	est.
- **Ejemplos válidos:**
  - eat(api): crea endpoint de listado de productos
  - eat(ui): añade selector de moneda USD y Bs
  - 	est(api): valida conversión de tasa DolarAPI

## 6. Pull Requests y Revisión de Código (@qa)
- **Título del PR:** Formato Conventional Commits (	ipo(alcance): descripción).
- **Cuerpo obligatorio del PR:**
  - ## Propósito
  - ## Conexión de Issue -> Debe incluir Closes #<id>
  - ## Instrucciones de Prueba
  - ## Checklist (- [x])
- **Criterios de auditoría (@qa):**
  - Nivel 0: Alineación exacta con el Issue, cero console.log() o código muerto, tamaño atómico.
  - Nivel Intermedio: Nombres explícitos, cero credenciales quemadas, principio DRY.
  - Nivel Experto: Sin consultas N+1 en EF Core, manejo resiliente de errores si DolarAPI falla.
  - Prefijos en hallazgos: locker: (impide fusión) y 
it: (mejora menor).
- **Fusión automática:** Una vez que @qa aprueba el PR y el check de GitHub Actions pasa en verde, @qa fusiona el PR (gh pr merge --squash --delete-branch) cerrando el Issue automáticamente.
