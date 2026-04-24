
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
{
    public class UpdatePedidoCommandHandler : IRequestHandler<UpdatePedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _uow;

       public UpdatePedidoCommandHandler(
        IPedidoRepository pedidoRepository, 
        IItemRepository itemRepository, 
        IUnitOfWork uow)
        {
            _pedidoRepository = pedidoRepository;
            _itemRepository = itemRepository;
            _uow = uow;
        }
        public async Task<bool> Handle(UpdatePedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id);
            if (pedido == null) return false;

            pedido.LimparItens(); // Remove os itens atuais do pedido

            var novosItens = await _itemRepository.GetByIdsAsync(request.ItensIds);

            foreach (var item in novosItens)
            {
                pedido.AdicionarItem(item);
            }

            _pedidoRepository.Update(pedido);
            return await _uow.Commit();
        }
    }
}