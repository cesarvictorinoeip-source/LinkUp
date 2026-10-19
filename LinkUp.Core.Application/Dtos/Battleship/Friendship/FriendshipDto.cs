namespace LinkUp.Core.Application.Dtos.Battleship.Friendship
{
    public class FriendshipDto
    {
        public int Id { get; set; }
        public string IdUserOne { get; set; } = null!;
        public string IdUserTwo { get; set; } = null!;
        public string UserOneName { get; set; } = string.Empty; // ✅
        public string UserTwoName { get; set; } = string.Empty; // ✅
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}