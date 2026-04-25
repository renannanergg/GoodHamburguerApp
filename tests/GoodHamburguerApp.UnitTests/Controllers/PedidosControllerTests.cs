using FluentAssertions;
using GoodHamburguerApp.Api.Controllers;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetPedidoById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Controllers
{
    public class PedidosControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PedidosController>> _loggerMock;
        private readonly PedidosController _controller;

        public PedidosControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PedidosController>>();
            _controller = new PedidosController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetById_DeveRetornarOk_QuandoPedidoExistir()
        {
            // Arrange
            var pedidoId = 1;
            var pedidoDto = new PedidoDTO(pedidoId, 100, 10, 90, new List<ItemDTO>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), default))
                         .ReturnsAsync(pedidoDto);

            // Act
            var result = await _controller.GetById(pedidoId);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); 
            _mediatorMock.Verify(m => m.Send(It.Is<GetPedidoByIdQuery>(q => q.Id == pedidoId), default), Times.Once);
        }

        [Fact]
        public async Task GetById_DeveRetornarNotFound_QuandoPedidoNaoExistir()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPedidoByIdQuery>(), default))
                         .ReturnsAsync((PedidoDTO)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_DeveRetornarCreatedAtAction_ComIdCorreto()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 2 } };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(123);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(PedidosController.GetById));
            createdResult.RouteValues["id"].Should().Be(123);
        }

        [Fact]
        public async Task Update_DeveRetornarBadRequest_QuandoIdsForemDiferentes()
        {
            // Arrange
            var idUrl = 1;
            var command = new UpdatePedidoCommand { Id = 2 }; // ID diferente

            // Act
            var result = await _controller.Update(idUrl, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("ID da URL difere do ID do corpo da requisição.");

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdatePedidoCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Update_DeveRetornarNoContent_QuandoSucesso()
        {
            // Arrange
            var id = 1;
            var command = new UpdatePedidoCommand { Id = id };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(id, command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound_QuandoNaoConcluido()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePedidoCommand>(), default))
                         .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
