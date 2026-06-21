using ByteShop.Catalog.Application.Dtos;

namespace ByteShop.Catalog.Application.Interfaces
{
    public interface IProdutoAppService
    {
        Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
        Task<ProdutoDto?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(CriarProdutoDto criarProdutoDto);
    }
}