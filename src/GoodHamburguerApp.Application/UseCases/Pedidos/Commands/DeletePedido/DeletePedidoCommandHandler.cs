

    using GoodHamburguerApp.Domain.Interfaces;
    using MediatR;

    namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
    {
    
        public class DeletePedidoCommandHandler : IRequestHandler<DeletePedidoCommand, bool>
        {
            private readonly IPedidoRepository _pedidoRepository;
            private readonly IUnitOfWork _uow;

            public DeletePedidoCommandHandler(IPedidoRepository pedidoRepository, IUnitOfWork uow)
            {
                _pedidoRepository = pedidoRepository;
                _uow = uow;
            }

            public async Task<bool> Handle(DeletePedidoCommand request, CancellationToken cancellationToken)
            {
                var pedido = await _pedidoRepository.GetByIdAsync(request.Id);
                if (pedido == null) return false;


                _pedidoRepository.Remove(pedido);
                
                return await _uow.Commit();
            }
        }
    }