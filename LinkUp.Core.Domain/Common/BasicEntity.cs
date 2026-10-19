namespace LinkUp.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        // El Id debe estar aquí para que todas las demás lo hereden
        public virtual int Id { get; set; }
    }
}