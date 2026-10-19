using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LinkUp.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        // Inicializar con string.Empty elimina el error CS9035
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        public string Password { get; set; } = string.Empty;

        // Propiedad para capturar el error y mostrarlo en la vista
        public string? Error { get; set; }
    }
}