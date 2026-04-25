using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Enums;
using GoodHamburguerApp.Infra.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburguerApp.IntegrationTests
{
    public class CardapioMockData
    {
        public static async Task CreateCardapio(CustomWebApplicationFactory factory, bool criar)
        {
            using var scope = factory.Services.CreateScope();

            var provider = scope.ServiceProvider;
            var db = provider.GetRequiredService<GoodHamburguerContext>();
            await db.Database.EnsureCreatedAsync();

            if (criar)
            {
                if (db.Itens.Any()) return;

                var itens = new List<Item>
                {
                    new Item("X Burger", 5.00m, CategoriaItem.Sanduiche, "Pão, carne bovina 150g e queijo cheddar."),
                    new Item("X Egg", 4.50m, CategoriaItem.Sanduiche, "Pão, carne bovina 150g, ovo frito e queijo cheddar."),
                    new Item("X Bacon", 7.00m, CategoriaItem.Sanduiche, "Pão, carne bovina 150g, bacon e queijo cheddar."),
                    new Item("Batata Frita", 2m, CategoriaItem.Batata, "Batata frita crocante"),
                    new Item("Refrigerante", 2.5m, CategoriaItem.Refrigerante, "Refrigerante gelado.")
                };

                await db.Itens.AddRangeAsync(itens);
                await db.SaveChangesAsync();
            }
            
        }

       
    }
}
