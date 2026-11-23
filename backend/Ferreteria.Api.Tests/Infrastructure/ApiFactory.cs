using System.Globalization;
using Ferreteria.Api.Features.Tasa;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ferreteria.Api.Tests.Infrastructure;

/// <summary>
/// Host de pruebas con base SQLite temporal (migrada al arrancar) y DolarAPI simulado,
/// para que ninguna prueba dependa de la red ni de la base de desarrollo.
/// </summary>
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public const decimal TasaRespaldo = 123.45m;

    private readonly string _rutaBaseDatos =
        Path.Combine(Path.GetTempPath(), $"ferreteria-pruebas-{Guid.NewGuid():N}.db");

    public StubDolarApiHandler DolarApi { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);
        builder.ConfigureAppConfiguration((_, configuracion) => configuracion.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = $"Data Source={_rutaBaseDatos}",
                ["Tasa:Url"] = "https://dolarapi.test/v1/dolares/oficial",
                ["Tasa:ValorRespaldo"] = TasaRespaldo.ToString(CultureInfo.InvariantCulture),
            }));

        builder.ConfigureTestServices(servicios =>
            servicios
                .AddHttpClient<ITasaDolarService, TasaDolarService>()
                .ConfigurePrimaryHttpMessageHandler(() => DolarApi));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }

        SqliteConnection.ClearAllPools();
        if (File.Exists(_rutaBaseDatos))
        {
            File.Delete(_rutaBaseDatos);
        }
    }
}
