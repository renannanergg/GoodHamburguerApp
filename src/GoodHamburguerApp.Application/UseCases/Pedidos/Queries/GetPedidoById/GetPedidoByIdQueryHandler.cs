using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetPedidoById
{
    public class GetPedidoByIdQueryHandler : IRequestHandler<GetPedidoByIdQuery, PedidoDTO?>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public GetPedidoByIdQueryHandler(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }
        public async Task<PedidoDTO?> Handle(GetPedidoByIdQuery request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(request.Id);

            if (pedido == null) return null;

            return _mapper.Map<PedidoDTO>(pedido);
        }
    }
}