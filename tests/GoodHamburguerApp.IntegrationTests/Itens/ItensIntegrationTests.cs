using FluentAssertions;
using GoodHamburguerApp.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburguerApp.IntegrationTests.Itens
{
    public class ItensIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ItensIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_CardapioCompleto_DeveRetornarTodosOsItens()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/itens/cardapio";
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedData<ItemDTO>>>();

            result.Success.Should().BeTrue();
            result.Data.Items.Should().NotBeEmpty();

            result.Data.Items.Any(i => i.Categoria == "Sanduiche").Should().BeTrue();
        }

       
    }
}
