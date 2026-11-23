using System.Net;
using Ferreteria.Api.Features.Tasa;
using Ferreteria.Api.Tests.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Ferreteria.Api.Tests;

/// <summary>
/// Pruebas del servicio de tasa con un <see cref="HttpMessageHandler"/> simulado:
/// éxito, error HTTP, timeout, caché y respaldo en cascada.
/// </summary>
public sealed class TasaDolarServiceTests
{
    private const string UrlDolarApi = "https://dolarapi.test/v1/dolares/oficial";
    private const decimal ValorRespaldo = 123.45m;

    [Fact]
    public async Task Devuelve_el_promedio_de_dolarapi_con_la_fuente_dolarapi()
    {
        var manejador = new StubDolarApiHandler();
        var servicio = CrearServicio(manejador, out _);

        var tasa = await servicio.ObtenerAsync(CancellationToken.None);

        Assert.Equal(StubDolarApiHandler.TasaEsperada, tasa.UsdBs);
        Assert.Equal(FuentesTasa.DolarApi, tasa.Fuente);
        Assert.Equal(
            new DateTimeOffset(2026, 11, 20, 0, 0, 0, TimeSpan.FromHours(-4)),
            tasa.FechaActualizacion);
        Assert.Equal(1, manejador.Llamadas);
    }

    [Fact]
    public async Task La_segunda_consulta_sale_de_la_cache_sin_llamar_al_tercero()
    {
        var manejador = new StubDolarApiHandler();
        var servicio = CrearServicio(manejador, out _);

        var primera = await servicio.ObtenerAsync(CancellationToken.None);
        var segunda = await servicio.ObtenerAsync(CancellationToken.None);

        Assert.Equal(FuentesTasa.DolarApi, primera.Fuente);
        Assert.Equal(FuentesTasa.Cache, segunda.Fuente);
        Assert.Equal(primera.UsdBs, segunda.UsdBs);
        Assert.Equal(1, manejador.Llamadas);
    }

    [Fact]
    public async Task Cuando_dolarapi_responde_con_error_http_usa_el_valor_de_respaldo()
    {
        var manejador = new StubDolarApiHandler
        {
            Responder = StubDolarApiHandler.RespuestaConEstado(HttpStatusCode.InternalServerError),
        };
        var servicio = CrearServicio(manejador, out _);

        var tasa = await servicio.ObtenerAsync(CancellationToken.None);

        Assert.Equal(FuentesTasa.Fallback, tasa.Fuente);
        Assert.Equal(ValorRespaldo, tasa.UsdBs);
    }

    [Fact]
    public async Task Cuando_dolarapi_agota_el_tiempo_usa_el_valor_de_respaldo()
    {
        var manejador = new StubDolarApiHandler { Responder = StubDolarApiHandler.RespuestaSinFin() };
        var servicio = CrearServicio(manejador, out _, timeoutSegundos: 1);
        using var cronometro = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var tasa = await servicio.ObtenerAsync(cronometro.Token);

        Assert.Equal(FuentesTasa.Fallback, tasa.Fuente);
        Assert.Equal(ValorRespaldo, tasa.UsdBs);
        Assert.Equal(1, manejador.Llamadas);
    }

    [Theory]
    [InlineData("esto no es json")]
    [InlineData("""{"moneda":"USD"}""")]
    [InlineData("""{"moneda":"USD","promedio":0}""")]
    [InlineData("""{"moneda":"USD","promedio":-8.5}""")]
    public async Task Cuando_dolarapi_devuelve_un_cuerpo_invalido_usa_el_valor_de_respaldo(string cuerpo)
    {
        var manejador = new StubDolarApiHandler
        {
            Responder = StubDolarApiHandler.RespuestaExitosa(cuerpo),
        };
        var servicio = CrearServicio(manejador, out _);

        var tasa = await servicio.ObtenerAsync(CancellationToken.None);

        Assert.Equal(FuentesTasa.Fallback, tasa.Fuente);
        Assert.Equal(ValorRespaldo, tasa.UsdBs);
    }

    [Fact]
    public async Task Si_falla_despues_de_un_exito_reutiliza_el_ultimo_valor_conocido()
    {
        var manejador = new StubDolarApiHandler();
        var servicio = CrearServicio(manejador, out var cache);

        var exitosa = await servicio.ObtenerAsync(CancellationToken.None);
        cache.Remove(TasaDolarService.ClaveCache);

        manejador.Responder = StubDolarApiHandler.RespuestaConEstado(HttpStatusCode.BadGateway);
        var degradada = await servicio.ObtenerAsync(CancellationToken.None);

        Assert.Equal(FuentesTasa.Fallback, degradada.Fuente);
        Assert.Equal(exitosa.UsdBs, degradada.UsdBs);
        Assert.NotEqual(ValorRespaldo, degradada.UsdBs);
    }

    [Fact]
    public async Task Propaga_la_cancelacion_del_cliente_y_no_la_confunde_con_una_falla()
    {
        var manejador = new StubDolarApiHandler { Responder = StubDolarApiHandler.RespuestaSinFin() };
        var servicio = CrearServicio(manejador, out _);
        using var cancelacion = new CancellationTokenSource();
        await cancelacion.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => servicio.ObtenerAsync(cancelacion.Token));
    }

    private static TasaDolarService CrearServicio(
        StubDolarApiHandler manejador,
        out MemoryCache cache,
        int timeoutSegundos = 5)
    {
        cache = new MemoryCache(new MemoryCacheOptions());
        var opciones = Options.Create(new TasaDolarOpciones
        {
            Url = UrlDolarApi,
            TimeoutSegundos = timeoutSegundos,
            CacheMinutos = 5,
            ValorRespaldo = ValorRespaldo,
        });

        return new TasaDolarService(
            new HttpClient(manejador),
            cache,
            opciones,
            NullLogger<TasaDolarService>.Instance);
    }
}
