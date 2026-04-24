
using AutoMapper;
using GoodHamburguerApp.Application.DTOs;
using GoodHamburguerApp.Domain.Entities;

namespace GoodHamburguerApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Item para ItemDTO
            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.ToString()));

            // Mapeamento de Pedido para PedidoDTO
            CreateMap<Pedido, PedidoDTO>();
        }
    }
}