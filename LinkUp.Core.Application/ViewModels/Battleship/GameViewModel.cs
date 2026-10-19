using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Player1Id { get; set; } = string.Empty;
        public string Player2Id { get; set; } = string.Empty;
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public string TurnPlayerId { get; set; } = string.Empty;
        public bool IsFinished { get; set; }
        public string? WinnerId { get; set; }
    }
}