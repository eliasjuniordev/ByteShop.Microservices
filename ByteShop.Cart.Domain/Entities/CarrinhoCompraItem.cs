using System.Text.Json.Serialization;

namespace ByteShop.Cart.Domain.Entities
{
    public class CarrinhoCompraItem
    {
        [JsonInclude]
        public Guid ProdutoId { get; private set; }

        [JsonInclude]
        public string ProdutoNome { get; private set; } = string.Empty;

        [JsonInclude]
        public decimal PrecoUnitario { get; private set; }

        [JsonInclude]
        public int Quantidade { get; private set; }

        [JsonInclude]
        public string ImagemUrl { get; private set; } = string.Empty;

        protected CarrinhoCompraItem() { }

        public CarrinhoCompraItem(Guid produtoId, string produtoNome, decimal precoUnitario, int quantidade, string imagemUrl)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
            ImagemUrl = imagemUrl;
        }
    }
}