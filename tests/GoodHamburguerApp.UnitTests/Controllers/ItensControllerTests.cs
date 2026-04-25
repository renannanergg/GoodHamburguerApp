using FluentAssertions;
using GoodHamburguerApp.Api.Controllers;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Itens.Queries;
using GoodHamburguerApp.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Controllers
{
    public class ItensControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<ItensController>> _loggerMock;
        private readonly ItensController _controller;

        public ItensControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ItensController>>();
            _controller = new ItensController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetCardapio_DeveRetornarOk_ComDadosPaginados()
        {
            // Arrange
            var offset = 0;
            var limit = 5;
            var itensDto = new List<ItemDTO>
            {
                new ItemDTO(1, "Sanduíche", 20.0m, CategoriaItem.Sanduiche.ToString(), "Sanduíche delicioso"),
                new ItemDTO(2, "Batata", 10.0m, CategoriaItem.Batata.ToString(), "Batata crocante")
            };

            var pagedData = new PagedData<ItemDTO>(itensDto, 2, offset, limit);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCardapioQuery>(), default))
                .ReturnsAsync(pagedData);

            // Act
            var result = await _controller.GetCardapio(offset, limit);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            _mediatorMock.Verify(m => m.Send(It.Is<GetCardapioQuery>(q =>
                q.Offset == offset && q.Limit == limit), default), Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Iniciando a consulta")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCardapio_DeveRetornarOk_MesmoQuandoCardapioEstiverVazio()
        {
            // Arrange
            var pagedDataVazio = new PagedData<ItemDTO>(new List<ItemDTO>(), 0, 0, 10);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCardapioQuery>(), default))
                .ReturnsAsync(pagedDataVazio);

            // Act
            var result = await _controller.GetCardapio(0, 10);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
