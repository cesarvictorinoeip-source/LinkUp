namespace LinkUp.Core.Domain.Entities
{
    public class Reaction : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string Type { get; set; } = string.Empty;
        public virtual AppUser? User { get; set; }
        public virtual Post? Post { get; set; }
    }
}