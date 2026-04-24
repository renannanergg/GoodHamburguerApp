
using GoodHamburguerApp.Domain.Entities;

namespace GoodHamburguerApp.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id);
        Task<IEnumerable<Pedido>> GetAllAsync();
        void Add(Pedido pedido);
        void Update(Pedido pedido);
        void Remove(Pedido pedido);

    }
}