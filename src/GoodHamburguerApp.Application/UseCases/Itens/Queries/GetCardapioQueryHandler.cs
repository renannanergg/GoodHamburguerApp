using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GoodHamburguerApp.Application.UseCases.Itens.Queries
{
    public class GetCardapioQueryHandler : IRequestHandler<GetCardapioQuery, PagedData<ItemDTO>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public GetCardapioQueryHandler(IItemRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<PagedData<ItemDTO>> Handle(GetCardapioQuery request, CancellationToken cancellationToken)
        {
            var (itens, totalCount) = await _repository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
            var itensDto = _mapper.Map<IReadOnlyList<ItemDTO>>(itens);
            return new PagedData<ItemDTO>(itensDto, totalCount, request.Offset, request.Limit);
        }
    }
}