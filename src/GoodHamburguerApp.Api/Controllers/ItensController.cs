

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
        private readonly ILogger _logger;
        public ItensController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("cardapio")]
        public async Task<IActionResult> GetCardapio()
        {
            _logger.LogInformation("Iniciando a consulta do cardápio.");

            var result = await _mediator.Send(new GetCardapioQuery());
            return CustomResponse(result, "Cardápio listado com sucesso.");
        }
    }
}