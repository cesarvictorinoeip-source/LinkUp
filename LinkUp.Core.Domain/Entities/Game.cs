namespace LinkUp.Core.Domain.Entities
{
    public class Game : BaseEntity
    {
        public required string Player1Id { get; set; }
        public virtual AppUser? Player1 { get; set; }
        public required string Player2Id { get; set; }
        public virtual AppUser? Player2 { get; set; }
        public required string PlayerTurnId { get; set; }
        public bool IsFinished { get; set; }
        public string? WinnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime LastMove { get; set; } = DateTime.Now;
        public virtual ICollection<Ship> Ships { get; set; } = new List<Ship>();
        public virtual ICollection<PlayerShot> Shots { get; set; } = new List<PlayerShot>();
    }
}
