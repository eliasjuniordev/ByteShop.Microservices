namespace ByteShop.BuildingBlocks.EventBus.Events
{

    public record PedidoCriadoIntegrationEvent
    {
        public string ClienteId { get; init; } = string.Empty;
        public Guid PedidoId { get; init; }
        public decimal ValorTotal { get; init; }


        public PedidoCriadoIntegrationEvent(string clienteId, Guid pedidoId, decimal valorTotal)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ValorTotal = valorTotal;
        }
    }
}