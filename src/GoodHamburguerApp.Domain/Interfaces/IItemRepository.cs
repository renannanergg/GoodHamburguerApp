using GoodHamburguerApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GoodHamburguerApp.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task<(IReadOnlyList<Item> Itens, int TotalCount)> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);
        Task<Item?> GetByIdAsync(int id);
        Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<int> ids);
    }
}