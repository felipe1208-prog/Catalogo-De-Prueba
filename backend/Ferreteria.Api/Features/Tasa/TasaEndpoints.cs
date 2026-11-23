namespace Ferreteria.Api.Features.Tasa;

public static class TasaEndpoints
{
    public static IEndpointRouteBuilder MapTasaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasa", async (ITasaDolarService servicio, CancellationToken cancellationToken) =>
            TypedResults.Ok(await servicio.ObtenerAsync(cancellationToken)))
            .WithTags("Tasa")
            .WithName("ObtenerTasaDolar");

        return app;
    }
}
