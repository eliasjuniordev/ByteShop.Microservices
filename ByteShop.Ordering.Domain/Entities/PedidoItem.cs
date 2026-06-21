namespace ByteShop.Ordering.Domain.Entities
{
    public class PedidoItem
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; } = string.Empty;
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }

        protected PedidoItem() { }

        public PedidoItem(Guid produtoId, string produtoNome, decimal precoUnitario, int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade do item deve ser maior que zero.");

            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }
}