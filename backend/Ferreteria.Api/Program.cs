using Ferreteria.Api.Data;
using Ferreteria.Api.Features.Categorias;
using Ferreteria.Api.Features.Productos;
using Ferreteria.Api.Features.Tasa;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

const string PoliticaCorsFrontend = "frontend-dev";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var cadenaConexion = builder.Configuration.GetConnectionString("Default") ?? "Data Source=Ferreteria.db";
builder.Services.AddDbContext<AppDbContext>(opciones => opciones.UseSqlite(cadenaConexion));

builder.Services.Configure<TasaDolarOpciones>(builder.Configuration.GetSection(TasaDolarOpciones.Seccion));
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<ITasaDolarService, TasaDolarService>((proveedor, cliente) =>
{
    var tasaOpciones = proveedor.GetRequiredService<IOptions<TasaDolarOpciones>>().Value;
    cliente.Timeout = TimeSpan.FromSeconds(Math.Max(1, tasaOpciones.TimeoutSegundos));
});

var origenesPermitidos = builder.Configuration.GetSection("Cors:OrigenesPermitidos").Get<string[]>()
    ?? ["http://localhost:5173"];
builder.Services.AddCors(opciones => opciones.AddPolicy(PoliticaCorsFrontend, politica => politica
    .WithOrigins(origenesPermitidos)
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors(PoliticaCorsFrontend);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapProductosEndpoints();
app.MapCategoriasEndpoints();
app.MapTasaEndpoints();

using (var alcance = app.Services.CreateScope())
{
    alcance.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.Run();

/// <summary>Punto de entrada expuesto para las pruebas de integración (WebApplicationFactory).</summary>
public partial class Program;
