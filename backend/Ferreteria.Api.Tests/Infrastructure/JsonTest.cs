using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Ferreteria.Api.Tests.Infrastructure;

/// <summary>Lectura de las respuestas JSON con nombres de propiedad explícitos del contrato.</summary>
internal static class JsonTest
{
    public static async Task<JsonObject> LeerObjetoAsync(this HttpResponseMessage respuesta) =>
        await respuesta.Content.ReadFromJsonAsync<JsonObject>()
            ?? throw new InvalidOperationException("La respuesta no contiene un objeto JSON.");

    public static async Task<JsonArray> LeerArregloAsync(this HttpResponseMessage respuesta) =>
        await respuesta.Content.ReadFromJsonAsync<JsonArray>()
            ?? throw new InvalidOperationException("La respuesta no contiene un arreglo JSON.");

    public static JsonObject Objeto(this JsonObject objeto, string propiedad) =>
        objeto[propiedad]!.AsObject();

    public static JsonArray Arreglo(this JsonObject objeto, string propiedad) =>
        objeto[propiedad]!.AsArray();

    public static int Entero(this JsonObject objeto, string propiedad) =>
        objeto[propiedad]!.GetValue<int>();

    public static decimal Decimal(this JsonObject objeto, string propiedad) =>
        objeto[propiedad]!.GetValue<decimal>();

    public static string Texto(this JsonObject objeto, string propiedad) =>
        objeto[propiedad]!.GetValue<string>();
}
