namespace Ferreteria.Api.Contracts;

public sealed record CategoriaResumenDto(int Id, string Nombre, string Slug);

/// <summary>Producto expuesto por la API; el dinero viaja siempre como <see cref="decimal"/>.</summary>
public sealed record ProductoDto(
    int Id,
    string Nombre,
    string Descripcion,
    decimal PrecioUsd,
    int CategoriaId,
    CategoriaResumenDto Categoria,
    string? ImagenUrl,
    int Stock,
    string Sku);

public sealed record ProductosPaginadosDto(
    IReadOnlyList<ProductoDto> Items,
    int Total,
    int Page,
    int PageSize);

public sealed record CategoriaDto(int Id, string Nombre, string Slug, int Conteo);

/// <summary>Tasa oficial USD → Bs. con la fuente efectiva del dato.</summary>
public sealed record TasaDto(decimal UsdBs, DateTimeOffset FechaActualizacion, string Fuente);
