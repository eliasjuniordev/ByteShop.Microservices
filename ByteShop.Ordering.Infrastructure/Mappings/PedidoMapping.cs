using ByteShop.Ordering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteShop.Ordering.Infrastructure.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClienteId)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.EnderecoEntrega)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                .HasConversion<int>(); 

            
            builder.HasMany(p => p.Itens)
                .WithOne()
                .HasForeignKey("PedidoId") 
                .OnDelete(DeleteBehavior.Cascade); 

         
            builder.Metadata.FindNavigation(nameof(Pedido.Itens))?
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}