

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Itens.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguerApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCardapio([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            _logger.LogInformation("Iniciando a consulta paginada do cardápio.");

            var result = await _mediator.Send(new GetCardapioQuery(offset, limit));

            return CustomResponse(result, "Cardápio listado com sucesso.");
        }
    }
}