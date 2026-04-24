
using FluentValidation;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands.CreatePedido
{
    public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
    {
        public CreatePedidoCommandValidator()
        {
            RuleFor(v => v.ItensIds)
                .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");
                
            RuleForEach(v => v.ItensIds)
                .GreaterThan(0).WithMessage("ID de item inválido.");
        }
    }
}