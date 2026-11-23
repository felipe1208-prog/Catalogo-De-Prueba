using System.Net;
using System.Text.Json.Nodes;
using Ferreteria.Api.Tests.Infrastructure;

namespace Ferreteria.Api.Tests;

public sealed class ProductosEndpointsTests : IClassFixture<ApiFactory>
{
    private const int TotalProductosSembrados = 40;
    private const int CategoriaElectricidad = 3;

    private readonly HttpClient _cliente;

    public ProductosEndpointsTests(ApiFactory fabrica) => _cliente = fabrica.CreateClient();

    [Fact]
    public async Task Listado_sin_filtros_devuelve_la_primera_pagina_por_defecto()
    {
        var respuesta = await _cliente.GetAsync("/api/productos");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var cuerpo = await respuesta.LeerObjetoAsync();
        Assert.Equal(TotalProductosSembrados, cuerpo.Entero("total"));
        Assert.Equal(1, cuerpo.Entero("page"));
        Assert.Equal(12, cuerpo.Entero("pageSize"));
        Assert.Equal(12, cuerpo.Arreglo("items").Count);

        var primerProducto = cuerpo.Arreglo("items")[0]!.AsObject();
        Assert.True(primerProducto.ContainsKey("precioUsd"));
        Assert.True(primerProducto.ContainsKey("descripcion"));
        Assert.True(primerProducto.ContainsKey("imagenUrl"));
        Assert.True(primerProducto.ContainsKey("stock"));
        Assert.True(primerProducto.ContainsKey("sku"));

        var categoriaDelPrimero = primerProducto.Objeto("categoria");
        Assert.Equal(primerProducto.Entero("categoriaId"), categoriaDelPrimero.Entero("id"));
        Assert.False(string.IsNullOrWhiteSpace(categoriaDelPrimero.Texto("nombre")));
        Assert.False(string.IsNullOrWhiteSpace(categoriaDelPrimero.Texto("slug")));
    }

    [Theory]
    [InlineData("taladro")]
    [InlineData("TALADRO")]
    [InlineData("Taladro")]
    public async Task Busqueda_por_texto_ignora_mayusculas(string termino)
    {
        var cuerpo = await BuscarAsync($"busqueda={termino}");

        Assert.Equal(1, cuerpo.Entero("total"));
        Assert.All(cuerpo.Arreglo("items"), item =>
            Assert.Contains("Taladro", item!.AsObject().Texto("nombre"), StringComparison.Ordinal));
    }

    [Fact]
    public async Task Busqueda_por_texto_ignora_tildes()
    {
        // "ELECTRIC" debe encontrar "Cable eléctrico", "Extensión eléctrica" y "Tablero eléctrico".
        var cuerpo = await BuscarAsync("busqueda=ELECTRIC");

        Assert.Equal(3, cuerpo.Entero("total"));
        Assert.All(cuerpo.Arreglo("items"), item =>
            Assert.Contains("léctric", item!.AsObject().Texto("nombre"), StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("plo-tub-012", "Tubo PVC 1/2 pulgada x 3 metros")]
    [InlineData("TOR-AUT", "Tornillo autoperforante 8 x 1 pulgada (100 unidades)")]
    public async Task Busqueda_encuentra_por_sku(string termino, string nombreEsperado)
    {
        var cuerpo = await BuscarAsync($"busqueda={termino}");

        Assert.Equal(1, cuerpo.Entero("total"));
        Assert.Equal(nombreEsperado, cuerpo.Arreglo("items")[0]!.AsObject().Texto("nombre"));
    }

    [Fact]
    public async Task Filtro_por_categoria_devuelve_solo_esa_categoria()
    {
        var cuerpo = await BuscarAsync($"categoriaId={CategoriaElectricidad}&pageSize=100");

        Assert.Equal(6, cuerpo.Entero("total"));
        Assert.All(cuerpo.Arreglo("items"), item =>
            Assert.Equal(CategoriaElectricidad, item!.AsObject().Entero("categoriaId")));
    }

    [Fact]
    public async Task Busqueda_y_categoria_se_combinan()
    {
        var cuerpo = await BuscarAsync($"busqueda=led&categoriaId={CategoriaElectricidad}");

        Assert.Equal(1, cuerpo.Entero("total"));
        Assert.Equal("Bombillo LED 9W luz blanca", cuerpo.Arreglo("items")[0]!.AsObject().Texto("nombre"));
    }

    [Theory]
    [InlineData("", 1, 12)]
    [InlineData("?page=0&pageSize=0", 1, 1)]
    [InlineData("?page=-4&pageSize=-9", 1, 1)]
    [InlineData("?pageSize=500", 1, 100)]
    [InlineData("?page=4&pageSize=7", 4, 7)]
    public async Task Paginacion_sanea_limites_y_ecos_en_la_respuesta(
        string parametros,
        int paginaEsperada,
        int tamanoEsperado)
    {
        var respuesta = await _cliente.GetAsync($"/api/productos{parametros}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var cuerpo = await respuesta.LeerObjetoAsync();
        Assert.Equal(paginaEsperada, cuerpo.Entero("page"));
        Assert.Equal(tamanoEsperado, cuerpo.Entero("pageSize"));
        Assert.True(cuerpo.Arreglo("items").Count <= tamanoEsperado);
    }

    [Fact]
    public async Task Paginacion_aplica_skip_y_take_sobre_el_orden_estable()
    {
        var todos = await BuscarAsync("pageSize=100");
        var idsEsperados = todos.Arreglo("items")
            .Skip(14)
            .Take(7)
            .Select(item => item!.AsObject().Entero("id"))
            .ToArray();

        var pagina = await BuscarAsync("page=3&pageSize=7");

        var idsObtenidos = pagina.Arreglo("items")
            .Select(item => item!.AsObject().Entero("id"))
            .ToArray();
        Assert.Equal(todos.Entero("total"), pagina.Entero("total"));
        Assert.Equal(idsEsperados, idsObtenidos);
    }

    [Fact]
    public async Task Detalle_devuelve_el_producto_con_su_categoria()
    {
        var respuesta = await _cliente.GetAsync("/api/productos/7");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var producto = await respuesta.LeerObjetoAsync();
        Assert.Equal(7, producto.Entero("id"));
        Assert.Equal("Esmeriladora angular 4 1/2 pulgadas", producto.Texto("nombre"));
        Assert.Equal(52.30m, producto.Decimal("precioUsd"));
        Assert.Equal(14, producto.Entero("stock"));
        Assert.Equal("HER-ESM-115", producto.Texto("sku"));

        var categoria = producto.Objeto("categoria");
        Assert.Equal(1, categoria.Entero("id"));
        Assert.Equal("Herramientas", categoria.Texto("nombre"));
        Assert.Equal("herramientas", categoria.Texto("slug"));
    }

    [Fact]
    public async Task Detalle_inexistente_devuelve_404_con_ProblemDetails()
    {
        var respuesta = await _cliente.GetAsync("/api/productos/999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
        Assert.Equal("application/problem+json", respuesta.Content.Headers.ContentType?.MediaType);

        var problema = await respuesta.LeerObjetoAsync();
        Assert.Equal(404, problema.Entero("status"));
        Assert.Equal("Producto no encontrado", problema.Texto("title"));
        Assert.Contains("999", problema.Texto("detail"), StringComparison.Ordinal);
        Assert.Equal("PRODUCTO_NO_ENCONTRADO", problema.Texto("codigo"));
        Assert.False(string.IsNullOrWhiteSpace(problema.Texto("traceId")));
    }

    private async Task<JsonObject> BuscarAsync(string query) =>
        await (await _cliente.GetAsync($"/api/productos?{query}")).LeerObjetoAsync();
}
