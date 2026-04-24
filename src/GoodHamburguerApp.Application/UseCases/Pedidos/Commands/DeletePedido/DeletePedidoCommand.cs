using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Pedidos.Commands
{
    public class DeletePedidoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        
        public DeletePedidoCommand(int id) => Id = id;
    }
}