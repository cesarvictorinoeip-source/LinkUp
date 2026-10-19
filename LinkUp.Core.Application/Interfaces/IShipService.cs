using LinkUp.Core.Application.Dtos.Battleship.Ship;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IShipService
    {
        Task<List<ShipDto>> GetPlayerShipsInGameAsync(int gameId, string userId);
    }
}