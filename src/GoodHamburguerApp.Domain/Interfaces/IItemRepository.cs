
using GoodHamburguerApp.Domain.Entities;

namespace GoodHamburguerApp.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<IEnumerable<Item>> GetByIdsAsync(IEnumerable<int> ids);
       
    }
}