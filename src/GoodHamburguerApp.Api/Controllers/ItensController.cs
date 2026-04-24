

using GoodHamburguerApp.Application.UseCases.Itens.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguerApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensController : MainController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ItensController> _logger;
        public ItensController(IMediator mediator, ILogger<ItensController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("cardapio")]
        public async Task<IActionResult> GetCardapio([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            _logger.LogInformation("Iniciando a consulta paginada do cardápio.");

            var result = await _mediator.Send(new GetCardapioQuery(offset, limit));
            return CustomResponse(result, "Cardápio listado com sucesso.");
        }
    }
}