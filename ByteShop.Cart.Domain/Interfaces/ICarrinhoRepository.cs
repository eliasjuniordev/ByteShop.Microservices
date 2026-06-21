using ByteShop.Cart.Domain.Entities;

namespace ByteShop.Cart.Domain.Interfaces
{
    public interface ICarrinhoRepository
    {
        Task<CarrinhoCompra?> ObterPorClienteIdAsync(string clienteId);
        Task<CarrinhoCompra> AtualizarAsync(CarrinhoCompra carrinho);
        Task<bool> LimparCarrinhoAsync(string clienteId);
    }
}