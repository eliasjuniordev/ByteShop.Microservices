namespace ByteShop.Ordering.Application.Dtos
{
    public class CriarPedidoItemDto
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
    }
}