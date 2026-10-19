using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace LinkUp.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public string? UserId { get; set; }
    }
}