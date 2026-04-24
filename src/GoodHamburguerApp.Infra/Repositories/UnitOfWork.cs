
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Infra.Context;

namespace GoodHamburguerApp.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GoodHamburguerContext _context;

        public UnitOfWork(GoodHamburguerContext context)
        {
            _context = context;
        }
        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}