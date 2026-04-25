using GoodHamburguerApp.Domain.Exceptions;

namespace GoodHamburguerApp.Domain.Entities
{
    public class Pedido : Entity
    {
        private readonly List<Item> _itens = new();
        public IReadOnlyCollection<Item> Itens => _itens.AsReadOnly();

        public decimal Subtotal  { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal Total { get; private set; }

        public Pedido() : base() { }

        public void AdicionarItem(Item item)
        {
            if (_itens.Any(i => i.Categoria == item.Categoria))
                throw new DomainException($"Item duplicado: Já existe um {item.Categoria} neste pedido.");

            _itens.Add(item);
            CalcularTotal();
        }

        public void RemoverItem(int itemId)
        {
            var item = _itens.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new DomainException("Item não encontrado no pedido.");

            _itens.Remove(item);
            CalcularTotal();
        }

        public void LimparItens()
        {
            _itens.Clear();
            Subtotal = 0;
            Desconto = 0;
            Total = 0;
        }

        private void CalcularTotal()
        {
            Subtotal = _itens.Sum(i => i.Preco);

            if (!_itens.Any())
            {
                Desconto = 0;
                Total = 0;
                return;
            }
            
            // Verifica se o pedido contém os itens necessários para aplicar os descontos
            var temSanduiche = _itens.Any(p => p.Categoria == Enums.CategoriaItem.Sanduiche);
            var temBatata = _itens.Any(p => p.Categoria == Enums.CategoriaItem.Batata);
            var temBebida = _itens.Any(p => p.Categoria == Enums.CategoriaItem.Refrigerante);

            // Lógica de Desconto 
            if (temSanduiche && temBatata && temBebida) Desconto = Subtotal * 0.20m;
            else if (temSanduiche && temBebida) Desconto = Subtotal * 0.15m;
            else if (temSanduiche && temBatata) Desconto = Subtotal * 0.10m;
            else Desconto = 0;

            Total = Subtotal - Desconto;
        }
    }
}