namespace LinkUp.Core.Domain.Common
{
    public abstract class BaseEntity
    {
        // El Id es virtual por si alguna clase necesita cambiar 

        // Propiedades de auditoría (Opcionales pero recomendadas)
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}