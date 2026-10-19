namespace LinkUp.Core.Application.Dtos.Battleship.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorImage { get; set; }
    }
}