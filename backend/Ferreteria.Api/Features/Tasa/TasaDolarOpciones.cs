namespace Ferreteria.Api.Features.Tasa;

/// <summary>Valores de <c>fuente</c> devueltos por <c>GET /api/tasa</c>.</summary>
public static class FuentesTasa
{
    public const string DolarApi = "dolarapi";
    public const string Cache = "cache";
    public const string Fallback = "fallback";
}

public sealed class TasaDolarOpciones
{
    public const string Seccion = "Tasa";

    public string Url { get; set; } = "https://ve.dolarapi.com/v1/dolares/oficial";

    /// <summary>Segundos máximos de espera antes de usar el respaldo.</summary>
    public int TimeoutSegundos { get; set; } = 5;

    /// <summary>Minutos que se conserva en caché la tasa obtenida de DolarAPI.</summary>
    public int CacheMinutos { get; set; } = 5;

    /// <summary>Tasa conservadora usada solo si nunca se ha obtenido un valor real.</summary>
    public decimal ValorRespaldo { get; set; } = 100m;
}
