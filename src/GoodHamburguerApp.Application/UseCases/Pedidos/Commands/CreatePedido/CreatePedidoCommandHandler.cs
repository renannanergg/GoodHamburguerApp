
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Exceptions;
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

            if (request.ItensIds == null || !request.ItensIds.Any())
            {
                throw new DomainException("O pedido deve conter pelo menos um item.");
            }

            var itens = await _itemRepository.GetByIdsAsync(request.ItensIds);

            if (itens == null || itens.Count() != request.ItensIds.Distinct().Count())
            {
                throw new DomainException("Um ou mais itens informados são inválidos.");
            }

            var pedido = new Pedido();

            foreach (var item in itens) pedido.AdicionarItem(item);

            _pedidoRepository.Add(pedido);
            
            var sucesso = await _uow.Commit();

            if (!sucesso)
                throw new DomainException("Não foi possível concluir o pedido. Tente novamente mais tarde.");

            return pedido.Id;
        }
    }
}