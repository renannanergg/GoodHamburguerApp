using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodHamburguerApp.Infra.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly GoodHamburguerContext _context;

        public ItemRepository(GoodHamburguerContext context)
        {
            _context = context;
        }
      
        public async Task<(IReadOnlyList<Item> Itens, int TotalCount)> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default)
        {
            var query = _context.Itens.AsNoTracking();
            var totalCount = await query.CountAsync(cancellationToken);
            var itens = await query
                .OrderBy(i => i.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);
            return (itens, totalCount);
        }

        public async Task<Item?> GetByIdAsync(int id) =>
            await _context.Itens.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<int> ids) =>
            await _context.Itens.Where(i => ids.Contains(i.Id)).ToListAsync();
    }
}