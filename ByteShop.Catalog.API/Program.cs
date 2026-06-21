using ByteShop.Catalog.Application.Interfaces;
using ByteShop.Catalog.Application.Services;
using ByteShop.Catalog.Domain.Interfaces;
using ByteShop.Catalog.Infrastructure.Context;
using ByteShop.Catalog.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);


BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));


builder.Services.AddSingleton<CatalogContext>();


builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoAppService, ProdutoAppService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ByteShop Catalog API v1"));
}

app.UseAuthorization();
app.MapControllers();

app.Run();