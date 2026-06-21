using ByteShop.Ordering.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteShop.Ordering.Infrastructure.Context
{
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
      
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}