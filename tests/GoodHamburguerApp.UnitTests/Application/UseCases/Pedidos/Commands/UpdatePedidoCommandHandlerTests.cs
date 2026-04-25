using Bogus;
using FluentAssertions;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Enums;
using GoodHamburguerApp.Domain.Exceptions;
using GoodHamburguerApp.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Commands
{
    public class UpdatePedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly UpdatePedidoCommandHandler _handler;
        private readonly Faker<Item> _itemFaker;

        public UpdatePedidoCommandHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _handler = new UpdatePedidoCommandHandler(
                _pedidoRepositoryMock.Object,
                _itemRepositoryMock.Object,
                _uowMock.Object);

            _itemFaker = new Faker<Item>()
                .CustomInstantiator(f => new Item(
                    f.Commerce.ProductName(),
                    f.Random.Decimal(10, 100),
                    f.PickRandom<CategoriaItem>(),
                    f.Commerce.ProductDescription()
                ));
        }

        [Fact]
        public async Task Handle_DeveAtualizarPedido_ComSucesso()
        {
            // Arrange
            var pedidoId = 2;
            var pedidoFake = new Pedido();

           
            var itemSanduiche = new Item("X-Burger", 25.0m, CategoriaItem.Sanduiche, "Pão e carne");
            var itemBatata = new Item("Batata G", 15.0m, CategoriaItem.Batata, "Batata frita");

            
            typeof(Entity).GetProperty("Id")?.SetValue(itemSanduiche, 1);
            typeof(Entity).GetProperty("Id")?.SetValue(itemBatata, 4);

            var novosItens = new List<Item> { itemSanduiche, itemBatata };
            var novosItensIds = novosItens.Select(i => i.Id).ToList();

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId)).ReturnsAsync(pedidoFake);

            _itemRepositoryMock.Setup(r => r.GetByIdsAsync(It.Is<List<int>>(ids => ids.Count == 2)))
                               .ReturnsAsync(novosItens);

            _uowMock.Setup(u => u.Commit()).ReturnsAsync(true);

            var command = new UpdatePedidoCommand { Id = pedidoId, ItensIds = novosItensIds };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            pedidoFake.Itens.Should().HaveCount(2);
            pedidoFake.Itens.Should().Contain(i => i.Categoria == CategoriaItem.Sanduiche);
            pedidoFake.Itens.Should().Contain(i => i.Categoria == CategoriaItem.Batata);

            _pedidoRepositoryMock.Verify(r => r.Update(pedidoFake), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoPedidoNaoExistir()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Pedido)null);
            var command = new UpdatePedidoCommand { Id = 99, ItensIds = new List<int> { 1 } };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

       
        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoFalharOCommit()
        {
            // Arrange
            var pedidoFake = new Pedido();
            var itens = _itemFaker.Generate(1);
            var ids = new List<int> { 1 };

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(pedidoFake);
            _itemRepositoryMock.Setup(r => r.GetByIdsAsync(ids)).ReturnsAsync(itens);
            _uowMock.Setup(u => u.Commit()).ReturnsAsync(false);

            var command = new UpdatePedidoCommand { Id = 1, ItensIds = ids };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("Não foi possível atualizar o pedido.");
        }
    }
}
