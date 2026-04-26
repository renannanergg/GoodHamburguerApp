using GoodHamburguerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburguerApp.Infra.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    { 
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Subtotal)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Desconto)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Total)
                .HasPrecision(18, 2)
                .IsRequired();

            // Tabela de ligação para a relação muitos-para-muitos entre Pedido e Item
            builder.HasMany(p => p.Itens)
                .WithMany(i => i.Pedidos)
                .UsingEntity(j => j.ToTable("PedidoItens")); // Nome da tabela no SQL

            var navigation = builder.Metadata.FindNavigation(nameof(Pedido.Itens));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}