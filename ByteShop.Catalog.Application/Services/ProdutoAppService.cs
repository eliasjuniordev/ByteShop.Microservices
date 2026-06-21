using ByteShop.Catalog.Application.Dtos;
using ByteShop.Catalog.Application.Interfaces;
using ByteShop.Catalog.Domain.Entities;
using ByteShop.Catalog.Domain.Interfaces;

namespace ByteShop.Catalog.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoAppService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
        {
            var produtos = await _produtoRepository.ObterTodosAsync();

            return produtos.Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                ImagemUrl = p.ImagemUrl
            });
        }

        public async Task<ProdutoDto?> ObterPorIdAsync(Guid id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);

            if (produto == null) return null;

            return new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                ImagemUrl = produto.ImagemUrl
            };
        }

        public async Task AdicionarAsync(CriarProdutoDto dto)
        {
            // Instancia a Entidade de Domínio disparando as validações internas de negócio
            var produto = new Produto(dto.Nome, dto.Descricao, dto.Preco, dto.QuantidadeEstoque, dto.ImagemUrl);

            await _produtoRepository.AdicionarAsync(produto);
        }
    }
}