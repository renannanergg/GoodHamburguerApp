using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguerApp.Infra.Repositories
{
    public class PedidosRepository : IPedidoRepository
    {
        private readonly GoodHamburguerContext _context;
        public PedidosRepository(GoodHamburguerContext context)
        {
            _context = context;
        }
        public void Add(Pedido pedido) => _context.Pedidos.Add(pedido);
        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .AsNoTracking() 
                .ToListAsync();
        }

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Pedido pedido) => _context.Pedidos.Update(pedido);

        public void Remove(Pedido pedido) => _context.Pedidos.Remove(pedido);
    }
}