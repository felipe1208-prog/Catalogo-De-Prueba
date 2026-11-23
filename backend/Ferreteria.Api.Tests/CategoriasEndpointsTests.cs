using Ferreteria.Api.Tests.Infrastructure;

namespace Ferreteria.Api.Tests;

public sealed class CategoriasEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _cliente;

    public CategoriasEndpointsTests(ApiFactory fabrica) => _cliente = fabrica.CreateClient();

    [Fact]
    public async Task Listado_devuelve_categorias_con_slug_y_conteo()
    {
        var respuesta = await _cliente.GetAsync("/api/categorias");

        Assert.Equal(System.Net.HttpStatusCode.OK, respuesta.StatusCode);
        var categorias = await respuesta.LeerArregloAsync();

        Assert.Equal(7, categorias.Count);
        Assert.All(categorias, nodo =>
        {
            var categoria = nodo!.AsObject();
            Assert.True(categoria.Entero("id") > 0);
            Assert.False(string.IsNullOrWhiteSpace(categoria.Texto("nombre")));
            Assert.False(string.IsNullOrWhiteSpace(categoria.Texto("slug")));
            Assert.True(categoria.Entero("conteo") > 0);
        });

        Assert.Equal(40, categorias.Sum(nodo => nodo!.AsObject().Entero("conteo")));

        var herramientas = categorias
            .Select(nodo => nodo!.AsObject())
            .Single(categoria => categoria.Texto("slug") == "herramientas");
        Assert.Equal("Herramientas", herramientas.Texto("nombre"));
        Assert.Equal(8, herramientas.Entero("conteo"));
    }

    [Fact]
    public async Task Los_slugs_son_unicos()
    {
        var categorias = await (await _cliente.GetAsync("/api/categorias")).LeerArregloAsync();

        var slugs = categorias.Select(nodo => nodo!.AsObject().Texto("slug")).ToArray();
        Assert.Equal(slugs.Length, slugs.Distinct(StringComparer.Ordinal).Count());
    }
}
