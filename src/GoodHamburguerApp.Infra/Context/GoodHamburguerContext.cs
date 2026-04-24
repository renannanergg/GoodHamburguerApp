using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguerApp.Infra.Context
{
    public class GoodHamburguerContext : DbContext
    {
        public GoodHamburguerContext(DbContextOptions<GoodHamburguerContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> Itens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodHamburguerContext).Assembly);
            
            // Seed Data dos Itens (Cardápio inicial)
            modelBuilder.Entity<Item>().HasData(
                new { Id = 1, Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaItem.Sanduiche, CreatedAt = DateTime.UtcNow, Descricao = "Pão, carne bovina 150g e queijo cheddar." },
                new { Id = 2, Nome = "X Egg", Preco = 4.50m, Categoria = CategoriaItem.Sanduiche, CreatedAt = DateTime.UtcNow, Descricao = "Pão, carne bovina 150g, ovo frito e queijo cheddar." },
                new { Id = 3, Nome = "X Bacon", Preco = 7.00m, Categoria = CategoriaItem.Sanduiche, CreatedAt = DateTime.UtcNow, Descricao = "Pão, carne bovina 150g, bacon e queijo cheddar." },
                new { Id = 4, Nome = "Batata frita", Preco = 2.00m, Categoria = CategoriaItem.Batata, CreatedAt = DateTime.UtcNow, Descricao = "Batata frita crocante." },
                new { Id = 5, Nome = "Refrigerante", Preco = 2.50m, Categoria = CategoriaItem.Refrigerante, CreatedAt = DateTime.UtcNow, Descricao = "Refrigerante gelado." }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}