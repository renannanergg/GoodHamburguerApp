using FluentAssertions;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Application.UseCases.Pedidos.Commands;
using GoodHamburguerApp.Infra.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace GoodHamburguerApp.IntegrationTests.Pedidos
{
    public class PedidosIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public PedidosIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test", "fake-token");
        }

        private async Task<int> CriarPedidoParaTeste(List<int> itensIds)
        {
            var command = new CreatePedidoCommand { ItensIds = itensIds };
            var response = await _client.PostAsJsonAsync("/api/v1/pedidos", command);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoIdResponse>>();
            return result.Data.Id;
        }

        #region Testes Metodo POST 
        [Fact]
        public async Task Post_PedidoComboCompleto_DeveRetornarSuccesso_E_PersistirNoBanco()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 4, 5 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoIdResponse>>();

            result.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Message.Should().Contain("sucesso");
            result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Post_PedidoLancheERefrigerante_DeveRetornarSuccesso_E_PersistirNoBanco()
        {

            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 5 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoIdResponse>>();

            result.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Message.Should().Contain("sucesso");
            result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Post_PedidoLancheEBatata_DeveRetornarSuccesso_E_PersistirNoBanco()
        {

            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 4 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoIdResponse>>();

            result.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Message.Should().Contain("sucesso");
            result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Post_PedidoSomenteLanche_DeveRetornarSuccesso_E_PersistirNoBanco()
        {

            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoIdResponse>>();

            result.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Message.Should().Contain("sucesso");
            result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }


        [Fact]
        public async Task Post_PedidoComItensDuplicados_DeveRetornarBadRequest()
        {

            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 2, 3 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Post_PedidoComItemInexistente_DeveRetornarBadRequest()
        {

            // Arrange

            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { 1, 2, 999 } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_PedidoComListaDeItensVazia_DeveRetornarBadRequest()
        {

            // Arrange

            await CardapioMockData.CreateCardapio(_factory, true);

            var url = "/api/v1/pedidos";

            var command = new CreatePedidoCommand { ItensIds = new List<int> { } };

            // Act 
            var response = await _client.PostAsJsonAsync(url, command);


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Testes Metodo GET

        [Fact]
        public async Task Get_Pedidos_DeveRetornarListaDePedidosComSucesso()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            // Criamos pelo menos dois pedidos para garantir que a lista venha preenchida
            var id1 = await CriarPedidoParaTeste(new List<int> { 1, 5 }); // X-Burger e Refrigerante
            var id2 = await CriarPedidoParaTeste(new List<int> { 2, 4, 5 }); //X-EGG e Batata e Refrigerante

            // Act
            var response = await _client.GetAsync("/api/v1/pedidos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedData<PedidoDTO>>>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Items.Should().NotBeEmpty();
            result.Data.Items.Select(p => p.Id).Should().Contain(new[] { id1, id2 });

            result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(2);

        }

        [Fact]
        public async Task Get_PedidosVazios_DeveRetornarListaVazia()
        {
            //Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GoodHamburguerContext>();
                context.Pedidos.RemoveRange(context.Pedidos);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/v1/pedidos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedData<PedidoDTO>>>();


            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Items.Should().BeEmpty();
            result.Data.TotalCount.Should().Be(0);

        }

        [Fact]
        public async Task GetById_PedidoExistente_DeveRetornarSucessoEDadosCorretos()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);

            var itensParaOPedido = new List<int> { 1, 4 };
            var idCriado = await CriarPedidoParaTeste(itensParaOPedido);

            // Act
            var response = await _client.GetAsync($"/api/v1/pedidos/{idCriado}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PedidoDTO>>();
            result.Should().NotBeNull();
            result.Data.Id.Should().Be(idCriado);
            result.Data.Itens.Should().HaveCount(itensParaOPedido.Count);
            result.Data.Itens.Select(i => i.Id).Should().Contain(itensParaOPedido);
        }

        [Fact]
        public async Task GetById_PedidoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);
            var idInexistente = 999999;

            // Act
            var response = await _client.GetAsync($"/api/v1/pedidos/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Testes Metodo PUT

        [Fact]
        public async Task Put_PedidoAtualizadoParaCombo_DeveAplicarDescontoCorreto()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);
            // Cria pedido inicial apenas com lanche (ID 1)
            var idPedido = await CriarPedidoParaTeste(new List<int> { 1 });

            // Novos itens: Sanduíche(1), Batata(4), Refrigerante(5) -> Combo 20%
            var command = new UpdatePedidoCommand
            {
                Id = idPedido,
                ItensIds = new List<int> { 1, 4, 5 }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/pedidos/{idPedido}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Validação de persistência e regra de negócio (Combo)
            var getResult = await _client.GetAsync($"/api/v1/pedidos/{idPedido}");
            var pedido = await getResult.Content.ReadFromJsonAsync<ApiResponse<PedidoDTO>>();


            pedido.Data.Total.Should().BeLessThan(pedido.Data.Subtotal); // Desconto aplicado
            pedido.Data.Itens.Should().HaveCount(3);
        }

        [Fact]
        public async Task Put_PedidoComDoisLanches_DeveRetornarBadRequest_PelaRegraDeDominio()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);
            var idPedido = await CriarPedidoParaTeste(new List<int> { 1 });


            var command = new UpdatePedidoCommand { Id = idPedido, ItensIds = new List<int> { 1, 2 } };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/pedidos/{idPedido}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);


        }

        [Fact]
        public async Task PedidoComDoisCombos_DeveRetornarBadRequest_PelaRegraDeDominio()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);
            var idPedido = await CriarPedidoParaTeste(new List<int> { 1 });

            
            var command = new UpdatePedidoCommand { Id = idPedido, ItensIds = new List<int> { 1, 2, 4, 4, 5, 5 } };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/pedidos/{idPedido}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_IdsDivergentes_DeveRetornarBadRequest()
        {
            // Arrange
            var idUrl = 10;
            var command = new UpdatePedidoCommand { Id = 20, ItensIds = new List<int> { 1 } };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/pedidos/{idUrl}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadAsStringAsync();
            error.Should().Contain("ID da URL difere");
        }

        #endregion


        #region Testes Metodo DELETE
        [Fact]
        public async Task Delete_PedidoExistente_DeveRetornarNoContent()
        {
            // Arrange
            await CardapioMockData.CreateCardapio(_factory, true);
           
            var idPedido = await CriarPedidoParaTeste(new List<int> { 3, 4 });

            // Act
            var response = await _client.DeleteAsync($"/api/v1/pedidos/{idPedido}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verificação de persistência: Tentar buscar o pedido deletado deve dar 404
            var getResponse = await _client.GetAsync($"/api/v1/pedidos/{idPedido}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_PedidoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = 9999;

            // Act
            var response = await _client.DeleteAsync($"/api/v1/pedidos/{idInexistente}");

            // Assert
         
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }

}

 public record PedidoIdResponse(int Id);

