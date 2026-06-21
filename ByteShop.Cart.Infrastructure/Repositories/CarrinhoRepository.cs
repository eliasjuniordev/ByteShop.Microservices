using System.Text.Json;
using ByteShop.Cart.Domain.Entities;
using ByteShop.Cart.Domain.Interfaces;
using StackExchange.Redis;

namespace ByteShop.Cart.Infrastructure.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly IDatabase _database;

        public CarrinhoRepository(IConnectionMultiplexer redis)
        {
       
            _database = redis.GetDatabase();
        }

        public async Task<CarrinhoCompra?> ObterPorClienteIdAsync(string clienteId)
        {
     
            var dados = await _database.StringGetAsync(clienteId);

            if (dados.IsNullOrEmpty)
                return null;

         
            return JsonSerializer.Deserialize<CarrinhoCompra>(dados!);
        }

        public async Task<CarrinhoCompra> AtuaizarAsync(CarrinhoCompra carrinho) 
        {
            var json = JsonSerializer.Serialize(carrinho);
            await _database.StringSetAsync(carrinho.ClienteId, json);

            return await ObterPorClienteIdAsync(carrinho.ClienteId) ?? carrinho;
        }

        public async Task<CarrinhoCompra> AtualizarAsync(CarrinhoCompra carrinho)
        {
         
            var json = JsonSerializer.Serialize(carrinho);

      
            await _database.StringSetAsync(carrinho.ClienteId, json);

            return await ObterPorClienteIdAsync(carrinho.ClienteId) ?? carrinho;
        }

        public async Task<bool> LimparCarrinhoAsync(string clienteId)
        {
          
            return await _database.KeyDeleteAsync(clienteId);
        }
    }
}
