
using GoodHamburguerApp.Application.DTOs;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetAllPedidos
{
    public class GetAllPedidosQuery : IRequest<PagedData<PedidoDTO>>
    {
        public int Offset { get; }
        public int Limit { get; }

        public GetAllPedidosQuery(int offset = 0, int limit = 10)
        {
            Offset = offset;
            Limit = limit;
        }
    }
    
}