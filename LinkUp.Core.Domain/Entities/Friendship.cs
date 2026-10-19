
namespace LinkUp.Core.Domain.Entities
{
    public class Friendship : BaseEntity
    {
        public required string IdUserOne { get; set; }
        public required string IdUserTwo { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual AppUser? Sender { get; set; }
        public virtual AppUser? Receiver { get; set; }
    }
}