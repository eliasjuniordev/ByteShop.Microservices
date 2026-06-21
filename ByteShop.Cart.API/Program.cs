using ByteShop.Cart.Application.Interfaces;
using ByteShop.Cart.Application.Services;
using ByteShop.Cart.Domain.Interfaces;
using ByteShop.Cart.Infrastructure.Repositories;
using MassTransit;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));


builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<ICarrinhoAppService, CarrinhoAppService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<ByteShop.Cart.Application.Consumers.PedidoCriadoConsumer>();

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

var app = builder.Build();

// --- CONFIGURAÇÃO DO PIPELINE HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ByteShop Cart API v1"));
}

app.UseAuthorization();
app.MapControllers();

app.Run();