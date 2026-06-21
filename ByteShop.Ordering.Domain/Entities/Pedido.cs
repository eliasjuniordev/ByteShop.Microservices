using ByteShop.Ordering.Domain.Enums;

namespace ByteShop.Ordering.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public string ClienteId { get; private set; } = string.Empty;
        public decimal ValorTotal { get; private set; }
        public DateTime DataPedido { get; private set; }
        public PedidoStatus Status { get; private set; }
        public string EnderecoEntrega { get; private set; } = string.Empty;

   
        private readonly List<PedidoItem> _itens = new();
        public IReadOnlyCollection<PedidoItem> Itens => _itens;

        protected Pedido() { }

        public Pedido(string clienteId, string enderecoEntrega, List<PedidoItem> itens)
        {
            if (string.IsNullOrWhiteSpace(clienteId))
                throw new ArgumentException("O cliente é obrigatório para gerar um pedido.");

            if (itens == null || !itens.Any())
                throw new ArgumentException("Não é possível criar um pedido sem itens.");

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            EnderecoEntrega = enderecoEntrega;
            Status = PedidoStatus.Rascunho;
            DataPedido = DateTime.UtcNow;

            _itens = itens;
            CalcularValorTotal();
        }

        public void MarcarComoPago()
        {
            if (Status != PedidoStatus.Rascunho)
                throw new InvalidOperationException("Apenas pedidos em rascunho podem ser pagos.");

            Status = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            if (Status == PedidoStatus.Enviado)
                throw new InvalidOperationException("Não é possível cancelar um pedido que já foi enviado.");

            Status = PedidoStatus.Cancelado;
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _itens.Sum(item => item.PrecoUnitario * item.Quantidade);
        }
    }
}