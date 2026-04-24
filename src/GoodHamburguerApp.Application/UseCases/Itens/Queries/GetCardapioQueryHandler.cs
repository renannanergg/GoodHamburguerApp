
using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Interfaces;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Itens.Queries
{
    public class GetCardapioQueryHandler : IRequestHandler<GetCardapioQuery, IEnumerable<ItemDTO>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public GetCardapioQueryHandler(IItemRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<IEnumerable<ItemDTO>> Handle(GetCardapioQuery request, CancellationToken cancellationToken)
        {
            var itens = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(itens);
        }
    }
}