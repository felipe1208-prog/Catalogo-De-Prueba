namespace Ferreteria.Api.Models;

/// <summary>Categoría del catálogo de ferretería (Herramientas, Plomería, Electricidad, etc.).</summary>
public sealed class Categoria
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    /// <summary>Identificador legible usado por el frontend en rutas y filtros.</summary>
    public required string Slug { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
