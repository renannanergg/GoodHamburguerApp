

using Bogus;
using FluentAssertions;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Exceptions;
using GoodHamburguerApp.Domain.Interfaces;
using Moq;
using System.Net;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Commands
{
    public class DeletePedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly DeletePedidoCommandHandler _handler;
        private readonly Faker _faker;

        public DeletePedidoCommandHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _faker = new Faker();
            _handler = new DeletePedidoCommandHandler(_pedidoRepositoryMock.Object, _uowMock.Object);
        }

        [Fact]
        public async Task Handle_DeveExcluirPedido_ComSucesso()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 1000); 
            var pedidoFake = new Pedido(); 

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
                .ReturnsAsync(pedidoFake);

            _uowMock.Setup(u => u.Commit()).ReturnsAsync(true);

            var command = new DeletePedidoCommand(pedidoId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _pedidoRepositoryMock.Verify(r => r.Remove(pedidoFake), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoPedidoNaoExistir()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 1000);
            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
                .ReturnsAsync((Pedido)null);

            var command = new DeletePedidoCommand(pedidoId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _uowMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoFalharOCommit()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 1000);
            var pedidoFake = new Pedido();

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
                .ReturnsAsync(pedidoFake);

            _uowMock.Setup(u => u.Commit()).ReturnsAsync(false);

            var command = new DeletePedidoCommand(pedidoId);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Não foi possível excluir o pedido no momento.");

            _pedidoRepositoryMock.Verify(r => r.Remove(pedidoFake), Times.Once);
        }
    }
}
