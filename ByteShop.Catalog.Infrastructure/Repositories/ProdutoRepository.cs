using ByteShop.Catalog.Domain.Entities;
using ByteShop.Catalog.Domain.Interfaces;
using ByteShop.Catalog.Infrastructure.Context;
using MongoDB.Driver;

namespace ByteShop.Catalog.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogContext _context;

        public ProdutoRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos
                .Find(produto => true)
                .ToListAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _context.Produtos
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.InsertOneAsync(produto);
        }

        public async Task AzureAtualizarAsync(Produto produto) 
        {
    
            await _context.Produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
        }

        public async Task AtualizarAsync(Produto produto)
        {
            await _context.Produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
        }

        public async Task RemoverAsync(Guid id)
        {
            await _context.Produtos.DeleteOneAsync(p => p.Id == id);
        }
    }
}