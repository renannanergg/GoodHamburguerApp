using AutoMapper;
using Bogus;
using FluentAssertions;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Pedidos.Queries.GetAllPedidos;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Queries
{
    public class GetAllPedidosQueryHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPedidosQueryHandler _handler;
        private readonly Faker<Pedido> _pedidoFaker;

        public GetAllPedidosQueryHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllPedidosQueryHandler(_pedidoRepositoryMock.Object, _mapperMock.Object);
            _pedidoFaker = new Faker<Pedido>();
        }

        [Fact]
        public async Task Handle_DeveRetornarPedidosPaginados_ComSucesso()
        {
            // Arrange
            var offset = 0;
            var limit = 5;
            var query = new GetAllPedidosQuery(offset, limit);

            var pedidosFake = _pedidoFaker.Generate(3);
            var pedidosDtoFake = pedidosFake.Select(p => new PedidoDTO(1, 100m, 10m, 90m, new List<ItemDTO>())).ToList();

            _pedidoRepositoryMock
                .Setup(r => r.GetAllAsync(offset, limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync((pedidosFake, 10)); 

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<PedidoDTO>>(pedidosFake))
                .Returns(pedidosDtoFake);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.TotalCount.Should().Be(10);
            result.Offset.Should().Be(offset);
            result.Limit.Should().Be(limit);

            _pedidoRepositoryMock.Verify(r => r.GetAllAsync(offset, limit, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVazia_QuandoNaoHouverPedidos()
        {
            // Arrange
            var query = new GetAllPedidosQuery(0, 5);
            var pedidosVazios = new List<Pedido>();
            var pedidosDtoVazios = new List<PedidoDTO>();

            _pedidoRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((pedidosVazios, 0));

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<PedidoDTO>>(pedidosVazios))
                .Returns(pedidosDtoVazios);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task Handle_DeveChamarMapperComOsPedidosCorretos()
        {
            // Arrange
            var query = new GetAllPedidosQuery(0, 10);
            var pedidosFake = _pedidoFaker.Generate(2);

            _pedidoRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((pedidosFake, 2));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<IReadOnlyList<PedidoDTO>>(pedidosFake), Times.Once);
        }
    }
}
