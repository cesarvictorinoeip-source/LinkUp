using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El comentario no puede estar vacío")]
        public string Content { get; set; } = string.Empty;

        public int PostId { get; set; }
        public string? UserId { get; set; }

        // Si este valor tiene ID, es una respuesta a otro comentario
        public int? ParentCommentId { get; set; }
    }
}