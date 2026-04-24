
namespace GoodHamburguerApp.Application.DTOs
{
   public record PedidoDTO(
    int Id, 
    decimal Subtotal, 
    decimal Desconto, 
    decimal Total, 
    List<ItemDTO> Itens);
}