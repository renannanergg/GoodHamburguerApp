using FluentAssertions;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Enums;
using GoodHamburguerApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Domain.Entities
{
    public class PedidoTests
    {
        private Item CriarItem(string nome, decimal preco, CategoriaItem categoria, int id = 0)
        {
            var item = new Item(nome, preco, categoria, "Descrição");
            typeof(Entity).GetProperty("Id")?.SetValue(item, id == 0 ? new Random().Next(1, 1000) : id);
            return item;
        }

        #region Cenários de Adição e Regras de Negócio

        [Fact]
        public void AdicionarItem_DeveLancarExcecao_QuandoCategoriaJaExiste()
        {
            // Arrange
            var pedido = new Pedido();
            var burger1 = CriarItem("X-Bacon", 7, CategoriaItem.Sanduiche);
            var burger2 = CriarItem("X-Egg", 4.5m, CategoriaItem.Sanduiche);

            // Act
            pedido.AdicionarItem(burger1);
            var act = () => pedido.AdicionarItem(burger2);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("*Item duplicado*Sanduiche*");
        }

        [Fact]
        public void RemoverItem_DeveAtualizarTotais_ComSucesso()
        {
            // Arrange
            var pedido = new Pedido();
            var item = CriarItem("Batata", 2, CategoriaItem.Batata, id: 1);
            pedido.AdicionarItem(item);

            // Act
            pedido.RemoverItem(1);

            // Assert
            pedido.Itens.Should().BeEmpty();
            pedido.Total.Should().Be(0);
            pedido.Subtotal.Should().Be(0);
        }

        [Fact]
        public void LimparItens_DeveResetarTodoOEstadoDoPedido()
        {
            // Arrange
            var pedido = new Pedido();
            pedido.AdicionarItem(CriarItem("X-Burger", 5, CategoriaItem.Sanduiche));
            pedido.AdicionarItem(CriarItem("Refrigerante", 2.5M, CategoriaItem.Refrigerante));

            // Act
            pedido.LimparItens();

            // Assert
            pedido.Itens.Should().BeEmpty();
            pedido.Total.Should().Be(0);
            pedido.Desconto.Should().Be(0);
        }

        #endregion

        #region Cenários de Cálculo de Desconto 

        [Fact]
        public void CalcularTotal_DeveAplicar20PorCento_QuandoComboCompleto()
        {
            // Combo: X-Burguer + Batata + Bebida
            var pedido = new Pedido();
            pedido.AdicionarItem(CriarItem("X-Burger", 5, CategoriaItem.Sanduiche)); // 5
            pedido.AdicionarItem(CriarItem("Batata", 2, CategoriaItem.Batata));     // 2
            pedido.AdicionarItem(CriarItem("Refrigerante", 2.5m, CategoriaItem.Refrigerante)); // 2.5
                                                                                      // Subtotal = 9.5. Desconto (20%) = 1.9. Total = 7.6.

            pedido.Subtotal.Should().Be(9.5m);
            pedido.Desconto.Should().Be(1.9m);
            pedido.Total.Should().Be(7.6m);
        }

        [Fact]
        public void CalcularTotal_DeveAplicar15PorCento_QuandoSanduicheEBebida()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(CriarItem("X-Burger", 5, CategoriaItem.Sanduiche));
            pedido.AdicionarItem(CriarItem("Refrigerante", 2.5m, CategoriaItem.Refrigerante));
            // Subtotal = 7.5. Desconto (15%) = 1.125 Total = 6.37

            pedido.Desconto.Should().Be(1.125m);
            pedido.Total.Should().Be(6.375m);
        }

        [Fact]
        public void CalcularTotal_DeveAplicar10PorCento_QuandoSanduicheEBatata()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(CriarItem("X-Burger", 5, CategoriaItem.Sanduiche));
            pedido.AdicionarItem(CriarItem("Batata", 2, CategoriaItem.Batata));
            // Subtotal = 7. Desconto (10%) = 0.7. Total = 6.3.

            pedido.Desconto.Should().Be(0.7m);
            pedido.Total.Should().Be(6.3m);
        }

        #endregion

    }
}
