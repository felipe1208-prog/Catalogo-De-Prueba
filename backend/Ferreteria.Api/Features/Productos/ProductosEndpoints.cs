using Ferreteria.Api.Contracts;
using Ferreteria.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ferreteria.Api.Features.Productos;

public static class ProductosEndpoints
{
    public const int PageSizePorDefecto = 12;
    public const int PageSizeMaximo = 100;

    public static IEndpointRouteBuilder MapProductosEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/productos").WithTags("Productos");

        grupo.MapGet("", ListarAsync).WithName("ListarProductos");
        grupo.MapGet("/{id:int}", ObtenerAsync).WithName("ObtenerProducto");

        return app;
    }

    private static async Task<Ok<ProductosPaginadosDto>> ListarAsync(
        AppDbContext db,
        string? busqueda,
        int? categoriaId,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var (pagina, tamanoPagina) = SanearPaginacion(page, pageSize);

        var consulta = db.Productos
            .AsNoTracking()
            .FiltrarPorBusqueda(busqueda)
            .FiltrarPorCategoria(categoriaId);

        var total = await consulta.CountAsync(cancellationToken);

        var items = await consulta
            .OrderBy(p => p.Nombre)
            .ThenBy(p => p.Id)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(ProyeccionProducto)
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(new ProductosPaginadosDto(items, total, pagina, tamanoPagina));
    }

    private static async Task<Results<Ok<ProductoDto>, ProblemHttpResult>> ObtenerAsync(
        int id,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var producto = await db.Productos
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ProyeccionProducto)
            .FirstOrDefaultAsync(cancellationToken);

        if (producto is null)
        {
            return TypedResults.Problem(
                title: "Producto no encontrado",
                detail: $"No existe un producto con id {id} en el catálogo.",
                statusCode: StatusCodes.Status404NotFound,
                extensions: new Dictionary<string, object?> { ["codigo"] = "PRODUCTO_NO_ENCONTRADO" });
        }

        return TypedResults.Ok(producto);
    }

    /// <summary>Proyección única para listado y detalle: una sola consulta con JOIN, sin N+1.</summary>
    private static readonly System.Linq.Expressions.Expression<Func<Models.Producto, ProductoDto>> ProyeccionProducto =
        p => new ProductoDto(
            p.Id,
            p.Nombre,
            p.Descripcion,
            p.PrecioUsd,
            p.CategoriaId,
            new CategoriaResumenDto(p.Categoria!.Id, p.Categoria.Nombre, p.Categoria.Slug),
            p.ImagenUrl,
            p.Stock,
            p.Sku);

    /// <summary>Sanea los parámetros de paginación: página ≥ 1 y tamaño entre 1 y 100 (12 por defecto).</summary>
    public static (int Pagina, int TamanoPagina) SanearPaginacion(int? page, int? pageSize) => (
        Math.Max(1, page ?? 1),
        Math.Clamp(pageSize ?? PageSizePorDefecto, 1, PageSizeMaximo));
}
