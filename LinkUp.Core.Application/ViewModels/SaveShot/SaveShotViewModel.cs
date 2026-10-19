using System.ComponentModel.DataAnnotations;

namespace LinkUp.Presentation.WebApp.Models.ViewModels.Battleship
{
    public class SaveShotViewModel
    {
        public int GameId { get; set; }
        public required string PlayerId { get; set; }

        [Required(ErrorMessage = "La fila es obligatoria")]
        [Range(0, 9, ErrorMessage = "Debe ser entre 0 y 9")]
        public int Row { get; set; }

        [Required(ErrorMessage = "La columna es obligatoria")]
        [Range(0, 9, ErrorMessage = "Debe ser entre 0 y 9")]
        public int Column { get; set; }
    }
}