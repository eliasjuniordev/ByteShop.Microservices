namespace ByteShop.Ordering.Application.Dtos
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public string ClienteId { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public List<PedidoItemDto> Itens { get; set; } = new();
    }
}