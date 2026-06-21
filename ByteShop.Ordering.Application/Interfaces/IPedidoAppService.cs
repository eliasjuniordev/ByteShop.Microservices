using ByteShop.Ordering.Application.Dtos;

namespace ByteShop.Ordering.Application.Interfaces
{
    public interface IPedidoAppService
    {
        Task<PedidoDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PedidoDto>> ObterPorClienteIdAsync(string clienteId);
        Task<PedidoDto> CriarPedidoAsync(CriarPedidoDto criarPedidoDto);
    }
}