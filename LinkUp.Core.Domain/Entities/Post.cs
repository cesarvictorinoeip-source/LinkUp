namespace LinkUp.Core.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;

        public virtual AppUser? User { get; set; } // ✅ AppUser en vez de User

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}