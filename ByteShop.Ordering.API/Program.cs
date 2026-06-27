using ByteShop.Ordering.Application.Interfaces;
using ByteShop.Ordering.Application.Services;
using ByteShop.Ordering.Domain.Interfaces;
using ByteShop.Ordering.Infrastructure.Context;
using ByteShop.Ordering.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DO EF CORE COM SQL SERVER ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderingContext>(options =>
    options.UseSqlServer(connectionString));

// --- INJEÇÃO DE DEPENDÊNCIA (DI) ---
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoAppService, PedidoAppService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CONFIGURAÇÃO DO MASSTRANSIT (RABBITMQ) ---
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Conecta na porta padrão de AMQP do RabbitMQ que configuramos no Docker
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


app.MapPrometheusScrapingEndpoint(); //

// --- CONFIGURAÇÃO DO PIPELINE HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ByteShop Ordering API v1"));
}

app.UseAuthorization();
app.MapControllers();

app.Run();