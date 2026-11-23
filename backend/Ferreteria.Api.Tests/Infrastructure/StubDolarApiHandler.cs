using System.Net;
using System.Text;

namespace Ferreteria.Api.Tests.Infrastructure;

/// <summary>
/// Sustituye a DolarAPI en las pruebas permitiendo elegir la respuesta del tercero
/// (éxito, código HTTP, timeout o JSON inválido) sin salir a la red.
/// </summary>
public sealed class StubDolarApiHandler : HttpMessageHandler
{
    public const string RespuestaValida = """
        {
          "moneda": "USD",
          "fuente": "oficial",
          "nombre": "Oficial",
          "compra": 915.25,
          "venta": 922.10,
          "promedio": 918.7662,
          "fechaActualizacion": "2026-11-20T00:00:00-04:00"
        }
        """;

    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> Responder { get; set; } =
        RespuestaExitosa();

    public int Llamadas { get; private set; }

    public static decimal TasaEsperada => 918.7662m;

    public static Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> RespuestaExitosa(
        string? cuerpo = null) =>
        (_, _) => Task.FromResult(Json(cuerpo ?? RespuestaValida));

    public static Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> RespuestaConEstado(
        HttpStatusCode estado) =>
        (_, _) => Task.FromResult(new HttpResponseMessage(estado));

    /// <summary>El tercero nunca contesta: la petición solo termina cuando se agota el timeout.</summary>
    public static Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> RespuestaSinFin() =>
        async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("La espera infinita no debe completarse.");
        };

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Llamadas++;
        return Responder(request, cancellationToken);
    }

    private static HttpResponseMessage Json(string cuerpo) => new(HttpStatusCode.OK)
    {
        Content = new StringContent(cuerpo, Encoding.UTF8, "application/json"),
    };
}
