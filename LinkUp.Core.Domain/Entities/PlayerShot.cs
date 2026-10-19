namespace LinkUp.Core.Domain.Entities
{
    public class PlayerShot : BaseEntity
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        public required string PlayerId { get; set; }
        public int Row { get; set; }        // ✅
        public int Column { get; set; }     // ✅
        public bool IsHit { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // ✅
    }
}