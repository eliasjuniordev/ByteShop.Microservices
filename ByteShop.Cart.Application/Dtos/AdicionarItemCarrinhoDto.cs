namespace ByteShop.Cart.Application.Dtos
{
    public class AdicionarItemCarrinhoDto
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
    }
}
