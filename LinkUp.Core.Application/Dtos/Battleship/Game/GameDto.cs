namespace LinkUp.Core.Application.Dtos.Battleship.Game
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Player1Id { get; set; } = null!;
        public string? Player1Name { get; set; }
        public string? Player2Id { get; set; }
        public string? Player2Name { get; set; }
        public string? CurrentTurnPlayerId { get; set; }
        public string? WinnerId { get; set; }
        public string Status { get; set; } = "Waiting";
        public bool IsFinished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}