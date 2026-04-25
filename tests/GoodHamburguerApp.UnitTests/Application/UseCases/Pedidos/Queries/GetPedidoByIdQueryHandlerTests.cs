using AutoMapper;
using FluentAssertions;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetPedidoById;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Queries
{
    public class GetPedidoByIdQueryHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPedidoByIdQueryHandler _handler;

        public GetPedidoByIdQueryHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetPedidoByIdQueryHandler(_pedidoRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPedidoDto_QuandoPedidoExistir()
        {
            // Arrange
            var pedidoId = 1;
            var query = new GetPedidoByIdQuery(pedidoId);
            var pedidoFake = new Pedido();

            var pedidoDtoFake = new PedidoDTO(pedidoId, 150m, 15m, 135m, new List<ItemDTO>());

            _pedidoRepositoryMock
                .Setup(r => r.GetByIdAsync(pedidoId))
                .ReturnsAsync(pedidoFake);

            _mapperMock
                .Setup(m => m.Map<PedidoDTO>(pedidoFake))
                .Returns(pedidoDtoFake);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(pedidoId);
            _pedidoRepositoryMock.Verify(r => r.GetByIdAsync(pedidoId), Times.Once);
            _mapperMock.Verify(m => m.Map<PedidoDTO>(pedidoFake), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoPedidoNaoExistir()
        {
            // Arrange
            var pedidoId = 99;
            var query = new GetPedidoByIdQuery(pedidoId);

            _pedidoRepositoryMock
                .Setup(r => r.GetByIdAsync(pedidoId))
                .ReturnsAsync((Pedido?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mapperMock.Verify(m => m.Map<PedidoDTO>(It.IsAny<Pedido>()), Times.Never);
        }
    }
}
