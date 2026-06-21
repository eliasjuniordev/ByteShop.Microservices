using ByteShop.Ordering.Domain.Entities;

namespace ByteShop.Ordering.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Pedido>> ObterPorClienteIdAsync(string clienteId);
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
    }
}