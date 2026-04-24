using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
{
    public class CreatePedidoCommand : IRequest<int>
    {
        public List<int> ItensIds { get; set; } = new();
    }
}