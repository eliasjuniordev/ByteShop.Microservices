using System.Text.Json.Serialization;
using ByteShop.Cart.Domain.Entities;

namespace ByteShop.Cart.Domain.Entities
{
    public class CarrinhoCompra
    {
        [JsonInclude]
        public string ClienteId { get; private set; } = string.Empty;

        [JsonInclude]
        public List<CarrinhoCompraItem> Itens { get; private set; } = new();

        [JsonIgnore] 
        public decimal PrecoTotal => Itens.Sum(item => item.PrecoUnitario * item.Quantidade);

        protected CarrinhoCompra() { }

        public CarrinhoCompra(string clienteId)
        {
            if (string.IsNullOrWhiteSpace(clienteId))
                throw new ArgumentException("O Id do cliente é obrigatório para criar um carrinho.");

            ClienteId = clienteId;
        }
    }
}