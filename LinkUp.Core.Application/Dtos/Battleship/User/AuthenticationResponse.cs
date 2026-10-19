using System;
using System.Collections.Generic;
namespace LinkUp.Core.Application.Dtos.Battleship.User
{
    public class AuthenticationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public bool HasError { get; set; }
        // Cambiamos a LISTA de errores para que soporte múltiples mensajes
        public List<string> Errors { get; set; } = new();
        public string? Error { get; set; }
    }
}