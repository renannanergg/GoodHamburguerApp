
using GoodHamburguerApp.Domain.Exceptions;
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
            if (pedido == null)
                return false;

            pedido.LimparItens(); 

            var novosItens = await _itemRepository.GetByIdsAsync(request.ItensIds);

            if (novosItens == null || novosItens.Count() != request.ItensIds.Distinct().Count())
            {
                throw new DomainException("Um ou mais itens informados são inválidos .");
            }

            foreach (var item in novosItens)
            {
                pedido.AdicionarItem(item);
            }

            _pedidoRepository.Update(pedido);
            var sucesso = await _uow.Commit();
            if (!sucesso)
                throw new DomainException("Não foi possível atualizar o pedido.");

            return true;
        }
    }
}