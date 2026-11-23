using System.Net;
using Ferreteria.Api.Tests.Infrastructure;

namespace Ferreteria.Api.Tests;

public sealed class TasaEndpointsTests
{
    [Fact]
    public async Task Devuelve_la_tasa_de_dolarapi_y_despues_la_sirve_de_cache()
    {
        using var fabrica = new ApiFactory();
        var cliente = fabrica.CreateClient();

        Assert.Equal(0, fabrica.DolarApi.Llamadas);

        var primera = await (await cliente.GetAsync("/api/tasa")).LeerObjetoAsync();

        Assert.Equal("dolarapi", primera.Texto("fuente"));
        Assert.Equal(StubDolarApiHandler.TasaEsperada, primera.Decimal("usdBs"));
        Assert.False(string.IsNullOrWhiteSpace(primera.Texto("fechaActualizacion")));

        var segunda = await (await cliente.GetAsync("/api/tasa")).LeerObjetoAsync();

        Assert.Equal("cache", segunda.Texto("fuente"));
        Assert.Equal(primera.Decimal("usdBs"), segunda.Decimal("usdBs"));
        Assert.Equal(1, fabrica.DolarApi.Llamadas);
    }

    [Theory]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task Nunca_responde_500_cuando_dolarapi_falla(HttpStatusCode estadoDelTercero)
    {
        using var fabrica = new ApiFactory();
        fabrica.DolarApi.Responder = StubDolarApiHandler.RespuestaConEstado(estadoDelTercero);
        var cliente = fabrica.CreateClient();

        var respuesta = await cliente.GetAsync("/api/tasa");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var tasa = await respuesta.LeerObjetoAsync();
        Assert.Equal(ApiFactory.TasaRespaldo, tasa.Decimal("usdBs"));
        Assert.Equal("fallback", tasa.Texto("fuente"));
    }

    [Fact]
    public async Task Usa_el_respaldo_si_dolarapi_devuelve_un_cuerpo_invalido()
    {
        using var fabrica = new ApiFactory();
        fabrica.DolarApi.Responder = StubDolarApiHandler.RespuestaExitosa("""{"moneda":"USD","promedio":0}""");
        var cliente = fabrica.CreateClient();

        var respuesta = await cliente.GetAsync("/api/tasa");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var tasa = await respuesta.LeerObjetoAsync();
        Assert.Equal("fallback", tasa.Texto("fuente"));
        Assert.Equal(ApiFactory.TasaRespaldo, tasa.Decimal("usdBs"));
    }
}
