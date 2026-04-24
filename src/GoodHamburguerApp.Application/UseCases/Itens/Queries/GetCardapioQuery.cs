using GoodHamburguerApp.Application.DTOs;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Itens.Queries;

public class GetCardapioQuery : IRequest<PagedData<ItemDTO>>
{
    public int Offset { get; }
    public int Limit { get; }

    public GetCardapioQuery(int offset = 0, int limit = 10)
    {
        Offset = offset;
        Limit = limit;
    }
}
