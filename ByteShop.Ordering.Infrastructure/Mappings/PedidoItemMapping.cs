using ByteShop.Ordering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteShop.Ordering.Infrastructure.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(i => i.PrecoUnitario)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Quantidade)
                .IsRequired();
        }
    }
}