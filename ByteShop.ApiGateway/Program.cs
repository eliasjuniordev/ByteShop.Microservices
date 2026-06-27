using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation(); // Coleta métricas nativas de requisições HTTP do .NET
        metrics.AddPrometheusExporter(); // Prepara os dados para o Prometheus raspar
    });

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint(); //


app.MapReverseProxy();

app.Run();
