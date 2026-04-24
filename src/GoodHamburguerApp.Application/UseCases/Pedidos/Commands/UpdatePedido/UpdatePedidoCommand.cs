using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
{
    public class UpdatePedidoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public List<int> ItensIds { get; set; } = new();
    }
}