using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Testcontainers.Redis;
using Xunit;

namespace ByteShop.Cart.Tests.IntegrationTests;

// O IAsyncLifetime permite-nos arrancar o contentor antes dos testes começarem
public class CartApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer;

    public CartApiFactory()
    {
        // Configuramos o Testcontainer para usar a imagem oficial do Redis
        _redisContainer = new RedisBuilder()
            .WithImage("redis:alpine")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
       
            var redisDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConnectionMultiplexer));
            if (redisDescriptor != null)
            {
                services.Remove(redisDescriptor);
            }


            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = _redisContainer.GetConnectionString();
                var configuration = ConfigurationOptions.Parse(connectionString, true);
                configuration.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(configuration);
            });
        });
    }


    public async Task InitializeAsync() => await _redisContainer.StartAsync();

    public new async Task DisposeAsync() => await _redisContainer.DisposeAsync();
}