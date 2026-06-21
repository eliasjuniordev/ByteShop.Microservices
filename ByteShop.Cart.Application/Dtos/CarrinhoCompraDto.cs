namespace ByteShop.Cart.Application.Dtos
{
    public class CarrinhoCompraDto
    {
        public string ClienteId { get; set; } = string.Empty;
        public List<CarrinhoCompraItemDto> Itens { get; set; } = new();
        public decimal PrecoTotal { get; set; }
    }
}