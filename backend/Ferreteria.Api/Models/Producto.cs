namespace Ferreteria.Api.Models;

/// <summary>Producto del catálogo. El dinero se expresa siempre en USD con <see cref="decimal"/>.</summary>
public sealed class Producto
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public required string Descripcion { get; set; }

    public decimal PrecioUsd { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

    public string? ImagenUrl { get; set; }

    public int Stock { get; set; }

    /// <summary>Código único de inventario (SKU), usado también por la búsqueda.</summary>
    public required string Sku { get; set; }
}
