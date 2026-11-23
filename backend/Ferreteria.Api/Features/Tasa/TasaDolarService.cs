using System.Text.Json;
using Ferreteria.Api.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ferreteria.Api.Features.Tasa;

public interface ITasaDolarService
{
    /// <summary>Devuelve la tasa oficial USD→Bs. Nunca lanza: si el tercero falla usa caché o respaldo.</summary>
    Task<TasaDto> ObtenerAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Obtiene la tasa oficial desde DolarAPI con caché en memoria de 5 minutos,
/// timeout acotado y respaldo en cascada (último valor conocido → constante conservadora).
/// </summary>
public sealed class TasaDolarService(
    HttpClient httpClient,
    IMemoryCache cache,
    IOptions<TasaDolarOpciones> opciones,
    ILogger<TasaDolarService> logger) : ITasaDolarService
{
    internal const string ClaveCache = "tasa:dolar:oficial";
    internal const string ClaveUltimoValor = "tasa:dolar:ultimo-valor";
    private static readonly TimeSpan VigenciaUltimoValor = TimeSpan.FromDays(30);

    public async Task<TasaDto> ObtenerAsync(CancellationToken cancellationToken)
    {
        var configuracion = opciones.Value;

        if (cache.TryGetValue(ClaveCache, out TasaDto? enCache) && enCache is not null)
        {
            return enCache with { Fuente = FuentesTasa.Cache };
        }

        try
        {
            var fresca = await ConsultarDolarApiAsync(configuracion, cancellationToken);
            cache.Set(ClaveCache, fresca, TimeSpan.FromMinutes(Math.Max(1, configuracion.CacheMinutos)));
            cache.Set(ClaveUltimoValor, fresca, VigenciaUltimoValor);
            return fresca;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // El cliente abortó la petición: se propaga, no es una falla del tercero.
            throw;
        }
        catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
        {
            logger.LogWarning(ex, "DolarAPI no respondió correctamente; se usará la tasa de respaldo.");
        }

        if (cache.TryGetValue(ClaveUltimoValor, out TasaDto? ultimoValor) && ultimoValor is not null)
        {
            return ultimoValor with { Fuente = FuentesTasa.Fallback };
        }

        return new TasaDto(
            configuracion.ValorRespaldo,
            DateTimeOffset.UtcNow,
            FuentesTasa.Fallback);
    }

    private async Task<TasaDto> ConsultarDolarApiAsync(
        TasaDolarOpciones configuracion,
        CancellationToken cancellationToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, configuracion.TimeoutSegundos)));

        using var respuesta = await httpClient.GetAsync(configuracion.Url, timeout.Token);
        respuesta.EnsureSuccessStatusCode();

        await using var contenido = await respuesta.Content.ReadAsStreamAsync(timeout.Token);
        using var documento = await JsonDocument.ParseAsync(contenido, cancellationToken: timeout.Token);

        var raiz = documento.RootElement;
        var tasa = LeerDecimal(raiz, "promedio")
            ?? LeerDecimal(raiz, "venta")
            ?? LeerDecimal(raiz, "compra");

        if (tasa is null or <= 0)
        {
            throw new JsonException("DolarAPI no devolvió una tasa válida.");
        }

        var fechaActualizacion = raiz.TryGetProperty("fechaActualizacion", out var fecha)
            && fecha.ValueKind == JsonValueKind.String
            && fecha.TryGetDateTimeOffset(out var valor)
                ? valor
                : DateTimeOffset.UtcNow;

        return new TasaDto(tasa.Value, fechaActualizacion, FuentesTasa.DolarApi);
    }

    private static decimal? LeerDecimal(JsonElement raiz, string propiedad) =>
        raiz.TryGetProperty(propiedad, out var valor)
        && valor.ValueKind == JsonValueKind.Number
        && valor.TryGetDecimal(out var numero)
            ? numero
            : null;
}
