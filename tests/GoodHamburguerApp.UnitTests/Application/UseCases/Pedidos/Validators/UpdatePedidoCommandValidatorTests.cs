using FluentAssertions;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands.UpdatePedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Pedidos.Validators
{
    public class UpdatePedidoCommandValidatorTests
    {
        private readonly UpdatePedidoCommandValidator _validator;

        public UpdatePedidoCommandValidatorTests()
        {
            _validator = new UpdatePedidoCommandValidator();
        }

        [Fact]
        public async Task Validador_DeveSerValido_QuandoComandoEstaCorreto()
        {
            // Arrange
            var command = new UpdatePedidoCommand
            {
                Id = 1,
                ItensIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Validador_DeveFalhar_QuandoIdForZeroOuNegativo()
        {
            // Arrange
            var command = new UpdatePedidoCommand { Id = 0, ItensIds = new List<int> { 1 } };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
        }

        [Theory]
        [InlineData(null)]
        public async Task Validador_DeveFalhar_QuandoListaDeItensForNula(List<int> itensIds)
        {
            // Arrange
            var command = new UpdatePedidoCommand { Id = 1, ItensIds = itensIds! };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("não pode ser nula"));
        }

        [Fact]
        public async Task Validador_DeveFalhar_QuandoListaDeItensEstiverVazia()
        {
            // Arrange
            var command = new UpdatePedidoCommand { Id = 1, ItensIds = new List<int>() };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("pelo menos um item"));
        }

        [Fact]
        public async Task Validador_DeveFalhar_QuandoHouverIdsDuplicados()
        {
            // Arrange
            var command = new UpdatePedidoCommand
            {
                Id = 1,
                ItensIds = new List<int> { 1, 2, 2 } // ID 2 duplicado
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("itens duplicados"));
        }

        [Fact]
        public async Task Validador_DeveFalhar_QuandoHouverIdDeItemInvalido()
        {
            // Arrange
            var command = new UpdatePedidoCommand
            {
                Id = 1,
                ItensIds = new List<int> { 1, 0, -5 }
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Any(e => e.ErrorMessage.Contains("ID de item inválido")).Should().BeTrue();
        }
    }
}
