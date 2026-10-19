using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorPicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }

        // Propiedad recursiva para respuestas (Hilos anidados)
        public List<CommentViewModel> Replies { get; set; } = new();
    }
}