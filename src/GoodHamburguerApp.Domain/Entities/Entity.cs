

namespace GoodHamburguerApp.Domain.Entities
{
    public abstract class Entity 
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    }
}