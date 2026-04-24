using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetAllPedidos
{
    public class GetAllPedidosQueryHandler : IRequestHandler<GetAllPedidosQuery, PagedData<PedidoDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public GetAllPedidosQueryHandler(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<PagedData<PedidoDTO>> Handle(GetAllPedidosQuery request, CancellationToken cancellationToken)
        {
            var (pedidos, totalCount) = await _pedidoRepository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
            var pedidosDto = _mapper.Map<IReadOnlyList<PedidoDTO>>(pedidos);
            return new PagedData<PedidoDTO>(pedidosDto, totalCount, request.Offset, request.Limit);
        }
    }
}