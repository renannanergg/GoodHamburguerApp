using FluentAssertions;
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

namespace GoodHamburguerApp.IntegrationTests.Auth
{
    public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public AuthIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _configuration = factory.Services.GetRequiredService<IConfiguration>();
        }

        [Fact]
        public async Task Login_ComCredenciaisCorretas_DeveRetornarToken()
        {
            // Arrange - Buscamos as credenciais configuradas no IConfiguration do teste
            var request = new LoginRequest(
                _configuration["Auth:Username"] ?? "admin",
                _configuration["Auth:Password"] ?? "admin123"
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Como você retorna return Ok(new { token }), lemos como um dicionário ou dynamic
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            result.Should().ContainKey("token");
            result["token"].Should().NotBeNullOrEmpty();
            result["token"].Split('.').Should().HaveCount(3); // Estrutura JWT
        }

        [Fact]
        public async Task Login_ComCredenciaisIncorretas_DeveRetornarUnauthorized()
        {
            // Arrange
            var request = new LoginRequest("usuario_errado", "senha_errada");

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            result["message"].Should().Be("Usuário ou senha inválidos.");
        }

        [Fact]
        public async Task RotaProtegida_ComTokenValido_DevePermitirAcesso()
        {
            //Arrange
            var credenciais = new LoginRequest(_configuration["Auth:Username"], _configuration["Auth:Password"]);
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", credenciais);
            var loginData = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = loginData["token"];

            //Act 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/v1/pedidos");

            //Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }
    }

    public record LoginRequest(string Username, string Password);
}
