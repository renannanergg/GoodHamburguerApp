using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GoodHamburguerApp.Application.UseCases.Itens.Queries
{
    public class GetCardapioQueryHandler : IRequestHandler<GetCardapioQuery, PagedData<ItemDTO>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CachePrefix = "Cardapio";
        private readonly ILogger<GetCardapioQueryHandler> _logger;

        public GetCardapioQueryHandler(IItemRepository repository, IMapper mapper, IMemoryCache cache, ILogger<GetCardapioQueryHandler> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<PagedData<ItemDTO>> Handle(GetCardapioQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{CachePrefix}_{request.Offset}_{request.Limit}";

            if (_cache.TryGetValue<PagedData<ItemDTO>>(cacheKey, out var cachedResult))
            {
                return cachedResult!;
            }

            var (itens, totalCount) = await _repository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
            var itensDto = _mapper.Map<IReadOnlyList<ItemDTO>>(itens);
            var result = new PagedData<ItemDTO>(itensDto, totalCount, request.Offset, request.Limit);

            var cacheOptions = new MemoryCacheEntryOptions()
             .SetAbsoluteExpiration(TimeSpan.FromHours(1))
             .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }
    }
}