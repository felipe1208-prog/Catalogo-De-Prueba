using System.Text;

namespace Ferreteria.Api.Data;

/// <summary>
/// Filtros del catálogo. Tanto el término de búsqueda como el texto de cada producto se normalizan
/// (minúsculas y sin diacríticos) para que la búsqueda por nombre/SKU ignore mayúsculas y tildes:
/// así "plomeria" encuentra "Plomería" y "TALADRO" encuentra "Taladro percutor".
/// </summary>
public static class ProductoFiltros
{
    /// <summary>
    /// Pares diacrítico → ASCII. SQLite solo aplica <c>lower()</c> a caracteres ASCII, por eso las
    /// vocales acentuadas mayúsculas también se mapean de forma explícita.
    /// </summary>
    private static readonly (char Original, char Reemplazo)[] Diacriticos =
    [
        ('á', 'a'), ('Á', 'a'),
        ('é', 'e'), ('É', 'e'),
        ('í', 'i'), ('Í', 'i'),
        ('ó', 'o'), ('Ó', 'o'),
        ('ú', 'u'), ('Ú', 'u'),
        ('ü', 'u'), ('Ü', 'u'),
        ('ñ', 'n'), ('Ñ', 'n'),
    ];

    public static IQueryable<Models.Producto> FiltrarPorBusqueda(
        this IQueryable<Models.Producto> productos,
        string? busqueda)
    {
        var termino = NormalizarTexto(busqueda);
        if (termino.Length == 0)
        {
            return productos;
        }

        // La cadena de Replace se mantiene explícita para que EF Core la traduzca a SQLite
        // (lower + replace anidados) y el filtrado siga ocurriendo en la base de datos.
        return productos.Where(p =>
            (p.Nombre + " " + p.Sku)
                .ToLower()
                .Replace("á", "a").Replace("Á", "a")
                .Replace("é", "e").Replace("É", "e")
                .Replace("í", "i").Replace("Í", "i")
                .Replace("ó", "o").Replace("Ó", "o")
                .Replace("ú", "u").Replace("Ú", "u")
                .Replace("ü", "u").Replace("Ü", "u")
                .Replace("ñ", "n").Replace("Ñ", "n")
                .Contains(termino));
    }

    public static IQueryable<Models.Producto> FiltrarPorCategoria(
        this IQueryable<Models.Producto> productos,
        int? categoriaId) =>
        categoriaId is > 0 ? productos.Where(p => p.CategoriaId == categoriaId) : productos;

    /// <summary>Normaliza en memoria el término escrito por el usuario (minúsculas y sin tildes).</summary>
    public static string NormalizarTexto(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return string.Empty;
        }

        var resultado = new StringBuilder(valor.Length);
        foreach (var caracter in valor.Trim().ToLowerInvariant())
        {
            var reemplazo = caracter;
            foreach (var (original, sustituto) in Diacriticos)
            {
                if (caracter == original)
                {
                    reemplazo = sustituto;
                    break;
                }
            }

            resultado.Append(reemplazo);
        }

        return resultado.ToString();
    }
}
