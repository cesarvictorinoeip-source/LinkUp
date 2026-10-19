using System;

namespace LinkUp.Core.Application.Dtos.Battleship.Reaction
{
    public class ReactionDto
    {
        public int Id { get; set; }

        // El ID de la publicación a la que se reacciona
        public int PostId { get; set; }

        // El ID del usuario que reacciona
        public string UserId { get; set; } = null!;

        // El nombre del usuario (para mostrar "A César le gusta esto")
        public string? UserName { get; set; }

        // Tipo de reacción: "Like", "Love", "Haha", etc.
        public string Type { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}