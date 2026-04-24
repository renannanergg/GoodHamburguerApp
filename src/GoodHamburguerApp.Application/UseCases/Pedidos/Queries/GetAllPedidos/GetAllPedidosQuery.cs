
using GoodHamburguerApp.Application.DTOs;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetAllPedidos
{
    public class GetAllPedidosQuery : IRequest<IEnumerable<PedidoDTO>> { }
    
}