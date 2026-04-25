using FluentValidation.TestHelper;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands.CreatePedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Validators
{
    public class CreatePedidoCommandValidatorTests
    {
        private readonly CreatePedidoCommandValidator _validator;

        public CreatePedidoCommandValidatorTests()
        {
            _validator = new CreatePedidoCommandValidator();
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_ItensIds_For_Nulo()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = null! };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ItensIds)
                  .WithErrorMessage("A lista de itens não pode ser nula.");
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_ItensIds_Estiver_Vazio()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = new List<int>() };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ItensIds)
                  .WithErrorMessage("O pedido deve conter pelo menos um item.");
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_Houver_Ids_Invalidos()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, -5, 0 } };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("ItensIds[1]");
            result.ShouldHaveValidationErrorFor("ItensIds[2]");
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_Houver_Ids_Duplicados()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 2, 1 } };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ItensIds)
                  .WithErrorMessage("O pedido não pode conter itens duplicados.");
        }

        [Fact]
        public void Deve_Passar_Quando_Comando_For_Valido()
        {
            // Arrange
            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 2, 3 } };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
