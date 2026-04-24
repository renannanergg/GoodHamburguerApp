

using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguerApp.Infra.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly GoodHamburguerContext _context;

        public ItemRepository(GoodHamburguerContext context)
        {
            _context = context;
        }
      
        public async Task<IEnumerable<Item>> GetAllAsync() => 
            await _context.Itens.AsNoTracking().ToListAsync();

        public async Task<Item?> GetByIdAsync(int id) =>
            await _context.Itens.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<int> ids) =>
            await _context.Itens.Where(i => ids.Contains(i.Id)).ToListAsync();
    }
}