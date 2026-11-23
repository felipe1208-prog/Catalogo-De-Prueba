using Ferreteria.Api.Contracts;
using Ferreteria.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ferreteria.Api.Features.Categorias;

public static class CategoriasEndpoints
{
    public static IEndpointRouteBuilder MapCategoriasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categorias", ListarAsync).WithTags("Categorías").WithName("ListarCategorias");

        return app;
    }

    private static async Task<Ok<IReadOnlyList<CategoriaDto>>> ListarAsync(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var categorias = await db.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto(c.Id, c.Nombre, c.Slug, c.Productos.Count))
            .ToListAsync(cancellationToken);

        return TypedResults.Ok<IReadOnlyList<CategoriaDto>>(categorias);
    }
}
