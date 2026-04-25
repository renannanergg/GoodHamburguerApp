using Xunit;
using Moq;
using FluentAssertions;
using Bogus;
using Microsoft.Extensions.Caching.Memory;
using GoodHamburguerApp.Application.UseCases.Itens.Queries;
using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Interfaces;
using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Enums;

namespace GoodHamburguerApp.UnitTests.Application.UseCases.Itens.Queries
{
    public class GetCardapioQueryHandlerTests
    {
        private readonly Mock<IItemRepository> _itensRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IMemoryCache _memoryCache;
        private readonly GetCardapioQueryHandler _handler;
        private readonly Faker<Item> _itemFaker;

        public GetCardapioQueryHandlerTests()
        {
            _itensRepositoryMock = new Mock<IItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetCardapioQueryHandler(
            _itensRepositoryMock.Object,
            _mapperMock.Object,
            _memoryCache);
            _itemFaker = new Faker<Item>()
                .CustomInstantiator(f => new Item(
                    f.Commerce.ProductName(),
                    f.Random.Decimal(5, 50),
                    f.PickRandom<CategoriaItem>(),
                    f.Commerce.ProductDescription()
                ));
        }

        [Fact]
        public async Task Handle_DeveRetornarDoCache_QuandoExistirDadosNaMemoria()
        {
            // Arrange
            var offset = 0;
            var limit = 5;
            var query = new GetCardapioQuery(offset, limit);
            var cacheKey = $"Cardapio_{query.Offset}_{query.Limit}";

            var itensDtoFakes = new List<ItemDTO> {
                new ItemDTO(1, "Burger Teste", 10m, "Descrição Teste", "Categoria Teste")
            };
            var pagedData = new PagedData<ItemDTO>(itensDtoFakes, 1, 0, 5);

           
            _memoryCache.Set(cacheKey, pagedData);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(pagedData);
            _itensRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveBuscarNoRepositorioEGravarNoCache_QuandoCacheEstiverVazio()
        {
            // Arrange
            var query = new GetCardapioQuery(0, 5);
            var itensFakes = _itemFaker.Generate(5);
            var itensDtoFakes = itensFakes.Select((i, idx) =>
                new ItemDTO(idx + 1, i.Nome, i.Preco, i.Descricao, i.Categoria.ToString())
            ).ToList();

            _itensRepositoryMock
                .Setup(r => r.GetAllAsync(query.Offset, query.Limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync((itensFakes, 5));

            _mapperMock
                .Setup(m => m.Map<IReadOnlyList<ItemDTO>>(itensFakes))
                .Returns(itensDtoFakes);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(5);
            _itensRepositoryMock.Verify(r => r.GetAllAsync(query.Offset, query.Limit, It.IsAny<CancellationToken>()), Times.Once);

            
            var cacheKey = $"Cardapio_{query.Offset}_{query.Limit}";
            _memoryCache.TryGetValue(cacheKey, out _).Should().BeTrue();
        }
    }
}
