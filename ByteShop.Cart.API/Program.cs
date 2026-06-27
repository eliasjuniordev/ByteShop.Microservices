using ByteShop.Cart.Application.Interfaces;
using ByteShop.Cart.Application.Services;
using ByteShop.Cart.Domain.Interfaces;
using ByteShop.Cart.Infrastructure.Repositories;
using MassTransit;
using OpenTelemetry.Metrics;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
    configuration.AbortOnConnectFail = false; // <-- A MAGIA ESTÁ AQUI
    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<ICarrinhoAppService, CarrinhoAppService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{

   
    x.AddConsumer<ByteShop.Cart.Application.Consumers.PedidoCriadoConsumer>(cfg =>
    {
       
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

        cfg.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
            cb.TripThreshold = 15;
            cb.ActiveThreshold = 5;
            cb.ResetInterval = TimeSpan.FromSeconds(30);
        });
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost:5672", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });


        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation(); // Coleta métricas nativas de requisições HTTP do .NET
        metrics.AddPrometheusExporter(); // Prepara os dados para o Prometheus raspar
    });


var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint(); //


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ByteShop Cart API v1"));
}

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }