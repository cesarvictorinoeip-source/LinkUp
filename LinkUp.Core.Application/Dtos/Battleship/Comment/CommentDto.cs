namespace LinkUp.Core.Application.Dtos.Battleship.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public int PostId { get; set; }
        public required string UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Propiedades de navegación para mostrar quién comentó
        public string? UserName { get; set; }
        public string? UserImageUrl { get; set; }
    }
}