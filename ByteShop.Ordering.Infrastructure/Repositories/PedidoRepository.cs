using ByteShop.Ordering.Domain.Entities;
using ByteShop.Ordering.Domain.Interfaces;
using ByteShop.Ordering.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ByteShop.Ordering.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly OrderingContext _context;

        public PedidoRepository(OrderingContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedido>> ObterPorClienteIdAsync(string clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AzureAtualizarAsync(Pedido pedido) 
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}