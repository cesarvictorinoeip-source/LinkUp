using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        // Estos campos son los que tu AutoMapper intenta llenar
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorPicture { get; set; }
        public int ReactionsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}