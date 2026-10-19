using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.ViewModels.Battleship
{
    public class DeleteGameViewModel
    {
        public int Id { get; set; }
        public string? Player1Name { get; set; }
        public string? Player2Name { get; set; }
    }
}