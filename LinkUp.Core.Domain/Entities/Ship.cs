// Ship.cs — CORREGIDO
namespace LinkUp.Core.Domain.Entities
{
    public class Ship : BaseEntity
    {
        public int GameId { get; set; }
        public required string PlayerId { get; set; }
        public required string Type { get; set; }
        public int Size { get; set; }
        public int StartX { get; set; }      // ✅ En vez de Coordinates string
        public int StartY { get; set; }
        public bool IsVertical { get; set; }
        public int Hits { get; set; }
        public bool IsSunk => Hits >= Size;

        public virtual Game Game { get; set; } = null!;
    }
}