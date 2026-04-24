

using GoodHamburguerApp.Application.DTOs;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetPedidoById
{
    public class GetPedidoByIdQuery : IRequest<PedidoDTO?>
    {
        public int Id { get; set; }

        public GetPedidoByIdQuery(int id)
        {
            Id = id;
        }
    }
}