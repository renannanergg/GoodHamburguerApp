

using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetAllPedidos;
using GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetPedidoById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguerApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : MainController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PedidosController> _logger;
        public PedidosController(IMediator mediator, ILogger<PedidosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            _logger.LogInformation("Iniciando consulta para listar pedidos.");

            var query = new GetAllPedidosQuery(offset, limit);
            var result = await _mediator.Send(query);
            return CustomResponse(result, "Pedidos listados com sucesso.");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Iniciando consulta para obter pedido com ID {Id}.", id);

            var result = await _mediator.Send(new GetPedidoByIdQuery(id));

            if (result == null) return NotFound();

            return CustomResponse(result, "Pedido encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePedidoCommand command)
        { 
            _logger.LogInformation("Iniciando criação de um novo pedido.");

            var id = await _mediator.Send(command);
            
            var response = new ApiResponse<object>(new { id }, "Pedido criado com sucesso!");

            _logger.LogInformation("Pedido criado com ID {Id}.", id);

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePedidoCommand command)
        {
            _logger.LogInformation("Iniciando atualização do pedido com ID {Id}.", id);

            if (id != command.Id) return BadRequest("ID da URL difere do ID do corpo da requisição.");

            var sucesso = await _mediator.Send(command);

            _logger.LogInformation(sucesso ? "Pedido com ID {Id} atualizado com sucesso." : "Pedido com ID {Id} não encontrado para atualização.", id);

            return sucesso ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Iniciando exclusão do pedido com ID {Id}.", id);

            var command = new DeletePedidoCommand(id);
            var sucesso = await _mediator.Send(command);

            _logger.LogInformation(sucesso ? "Pedido com ID {Id} excluído com sucesso." : "Pedido com ID {Id} não encontrado para exclusão.", id);
            return sucesso ? NoContent() : NotFound();
        }
    }
}