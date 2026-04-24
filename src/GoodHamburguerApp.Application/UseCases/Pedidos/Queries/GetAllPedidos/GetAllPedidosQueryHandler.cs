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
    public class GetAllPedidosQueryHandler : IRequestHandler<GetAllPedidosQuery, IEnumerable<PedidoDTO>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public GetAllPedidosQueryHandler(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PedidoDTO>> Handle(GetAllPedidosQuery request, CancellationToken cancellationToken)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
        }
    }
}