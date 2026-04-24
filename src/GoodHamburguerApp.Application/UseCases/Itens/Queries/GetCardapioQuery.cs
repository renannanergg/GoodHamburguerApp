
using GoodHamburguerApp.Application.DTOs;
using MediatR;

namespace GoodHamburguerApp.Application.UseCases.Itens.Queries;

public class GetCardapioQuery : IRequest<IEnumerable<ItemDTO>> { }
   
