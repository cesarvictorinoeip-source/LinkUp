namespace LinkUp.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        // El Id debe estar aquí para que todas las demás lo hereden
        public virtual int Id { get; set; }
    }
}
// Eliminar la definición duplicada de BaseEntity en este archivo
// Si BasicDto necesita heredar de BaseEntity, asegúrate de que el archivo BaseEntity.cs esté referenciado correctamente.
