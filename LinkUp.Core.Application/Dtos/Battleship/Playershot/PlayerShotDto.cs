namespace LinkUp.Core.Application.Dtos.Battleship.Playershot
{
    public class PlayerShotDto
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string PlayerId { get; set; } = string.Empty;
        public int GameId { get; set; }
        public bool IsHit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}