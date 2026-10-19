using System;

namespace LinkUp.Core.Application.Dtos.Battleship.Ship
{
    public class ShipDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string PlayerId { get; set; } = null!;
        public string Type { get; set; } = null!; // Ejemplo: "Submarino", "Portaaviones"
        public int Size { get; set; }

        // Coordenadas de inicio
        public int StartX { get; set; }
        public int StartY { get; set; }

        // Orientación: true si es vertical, false si es horizontal
        public bool IsVertical { get; set; }

        // Cantidad de veces que ha sido impactado
        public int Hits { get; set; }

        // Propiedad calculada: si los impactos igualan al tamaño, está hundido
        public bool IsSunk => Hits >= Size;
    }
}