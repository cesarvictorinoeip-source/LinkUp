using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Domain.Common.Enums
{
    public enum GameStatus
    {
        PlacingShips = 1, // Fase de posicionamiento
        InProgress = 2,   // Fase de ataque
        Finished = 3,     // Alguien ganó
        Abandoned = 4     // Pasaron las 48 horas
    }
}
