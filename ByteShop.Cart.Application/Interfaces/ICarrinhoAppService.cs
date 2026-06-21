using ByteShop.Cart.Application.Dtos;

namespace ByteShop.Cart.Application.Interfaces
{
    public interface ICarrinhoAppService
    {
        Task<CarrinhoCompraDto?> ObterPorClienteIdAsync(string clienteId);
        Task<CarrinhoCompraDto> AdicionarItemAsync(string clienteId, AdicionarItemCarrinhoDto itemDto);
        Task<CarrinhoCompraDto?> RemoverItemAsync(string clienteId, Guid produtoId);
        Task LimparCarrinhoAsync(string clienteId);
    }
}