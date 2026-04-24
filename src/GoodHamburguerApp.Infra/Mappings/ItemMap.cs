
using GoodHamburguerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburguerApp.Infra.Mappings
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Itens");
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.Nome).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Descricao).HasMaxLength(255);
            builder.Property(i => i.Preco).HasPrecision(18, 2);

            builder.Property(i => i.Categoria)
               .HasConversion<string>() 
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}