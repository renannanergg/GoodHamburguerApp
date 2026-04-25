using Xunit;
using Moq;
using FluentAssertions;
using Bogus;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Domain.Enums;
using GoodHamburguerApp.Domain.Exceptions;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Commands
{
    public class CreatePedidoCommandHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly CreatePedidoHandler _handler;
        private readonly Faker<Item> _itemFaker;

        public CreatePedidoCommandHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _handler = new CreatePedidoHandler(_pedidoRepositoryMock.Object, _itemRepositoryMock.Object, _uowMock.Object);
            _itemFaker = new Faker<Item>()
                .CustomInstantiator(f => new Item(
                    f.Commerce.ProductName(),
                    f.Random.Decimal(5, 50),
                    f.PickRandom<CategoriaItem>(),
                    f.Commerce.ProductDescription()
                ));
        }

        [Fact]
        public async Task Handle_DeveCriarPedido_ComSucesso()
        {
            // Arrange
            var itens = new List<Item>
            {
                new Item("X-Burger", 5, CategoriaItem.Sanduiche, "TESTE"),
                new Item("Batata", 2, CategoriaItem.Batata, "TESTE"),
                new Item("Refrigerante", 2.5m, CategoriaItem.Refrigerante, "TESTE")
            };

            var itensIds = new List<int> { 1, 2, 3 };

            for (int i = 0; i < itens.Count; i++)
                typeof(Item).GetProperty("Id").SetValue(itens[i], itensIds[i]);

            _itemRepositoryMock.Setup(r => r.GetByIdsAsync(itensIds)).ReturnsAsync(itens);
            _uowMock.Setup(u => u.Commit()).ReturnsAsync(true);


            _pedidoRepositoryMock
            .Setup(r => r.Add(It.IsAny<Pedido>()))
            .Callback<Pedido>(p =>
            {
                typeof(Pedido).GetProperty("Id").SetValue(p, 123); 
            });

            var command = new CreatePedidoCommand { ItensIds = itensIds };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(123);
            _pedidoRepositoryMock.Verify(r => r.Add(It.IsAny<Pedido>()), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoItensDuplicadosPorCategoria()
        {
            // Arrange
            var sanduiche = new Item("Sanduíche X", 20, CategoriaItem.Sanduiche, "Delicioso");
            var sanduiche2 = new Item("Sanduíche Y", 22, CategoriaItem.Sanduiche, "Outro");
            typeof(Item).GetProperty("Id").SetValue(sanduiche, 1);
            typeof(Item).GetProperty("Id").SetValue(sanduiche2, 2);
            var itens = new List<Item> { sanduiche, sanduiche2 };
            var itensIds = new List<int> { 1, 2 };
            _itemRepositoryMock.Setup(r => r.GetByIdsAsync(itensIds)).ReturnsAsync(itens);
            var command = new CreatePedidoCommand { ItensIds = itensIds };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Item duplicado: Já existe um Sanduiche neste pedido.");
            _pedidoRepositoryMock.Verify(r => r.Add(It.IsAny<Pedido>()), Times.Never);
            _uowMock.Verify(u => u.Commit(), Times.Never);
        }


        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoFalharAoPersistirNoBanco()
        {
            // Arrange
            var item1 = new Item("Hambúrguer", 20.0m, CategoriaItem.Sanduiche, "Descrição");
            var item2 = new Item("Refrigerante", 10.0m, CategoriaItem.Refrigerante, "Descrição");

            var itens = new List<Item> { item1, item2 };
            var itensIds = new List<int> { 1, 2 };

            _itemRepositoryMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(itens);

            // Simulamos que o banco de dados falhou ao salvar (Commit retorna false)
            _uowMock.Setup(u => u.Commit()).ReturnsAsync(false);

            var command = new CreatePedidoCommand { ItensIds = itensIds };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Não foi possível concluir o pedido. Tente novamente mais tarde.");

            _pedidoRepositoryMock.Verify(r => r.Add(It.IsAny<Pedido>()), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

    }
}
