
using FluentValidation;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands.CreatePedido
{
    public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
    {
        public CreatePedidoCommandValidator()
        {
            RuleFor(v => v.ItensIds)
             .NotNull().WithMessage("A lista de itens não pode ser nula.")
             .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
             .Must(x => x != null && x.Count == x.Distinct().Count())
             .WithMessage("O pedido não pode conter itens duplicados.");

            RuleForEach(v => v.ItensIds)
                .GreaterThan(0).WithMessage("ID de item inválido.");
        }
    }
}