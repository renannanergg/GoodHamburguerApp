using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands.UpdatePedido
{
    public class UpdatePedidoCommandValidator : AbstractValidator<UpdatePedidoCommand>
    {
        public UpdatePedidoCommandValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0).WithMessage("O ID do pedido é obrigatório.");

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
