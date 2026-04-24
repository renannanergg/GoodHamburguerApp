
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
{
    public class CreatePedidoHandler : IRequestHandler<CreatePedidoCommand, int>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _uow;
        public CreatePedidoHandler(IPedidoRepository pedidoRepository, IItemRepository itemRepository, IUnitOfWork uow)
        {
            _pedidoRepository = pedidoRepository;
            _itemRepository = itemRepository;
            _uow = uow;
        }
        public async Task<int> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = new Pedido();

            var itens = await _itemRepository.GetByIdsAsync(request.ItensIds);
            foreach (var item in itens) pedido.AdicionarItem(item);

            _pedidoRepository.Add(pedido);
            
            await _uow.Commit(); 
            
            return pedido.Id;
        }
    }
}