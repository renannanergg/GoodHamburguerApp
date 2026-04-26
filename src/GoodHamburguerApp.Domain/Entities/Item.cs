using GoodHamburguerApp.Domain.Enums;

namespace GoodHamburguerApp.Domain.Entities
{
    public class Item : Entity
    {
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public CategoriaItem Categoria { get; private set; }
        public string Descricao { get; private set; }

       
        private readonly List<Pedido> _pedidos = new();
        public IReadOnlyCollection<Pedido> Pedidos => _pedidos;

        private Item() { }

        public Item(string nome, decimal preco, CategoriaItem categoria, string descricao)
        {
            Nome = nome;
            Preco = preco;
            Categoria = categoria;
            Descricao = descricao;
        }
    }
}