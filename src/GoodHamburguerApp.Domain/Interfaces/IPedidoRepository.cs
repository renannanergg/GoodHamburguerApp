
using GoodHamburguerApp.Domain.Entities;

namespace GoodHamburguerApp.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id);
        Task<(IReadOnlyList<Pedido> Pedidos, int TotalCount)> GetAllAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);
        void Add(Pedido pedido);
        void Update(Pedido pedido);
        void Remove(Pedido pedido);

    }
}