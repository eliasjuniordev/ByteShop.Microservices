namespace ByteShop.Ordering.Application.Dtos
{
    public class CriarPedidoDto
    {
        public string ClienteId { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public List<CriarPedidoItemDto> Itens { get; set; } = new();
    }
}