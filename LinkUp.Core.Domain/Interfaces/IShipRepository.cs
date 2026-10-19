using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;

namespace LinkUp.Core.Application.Interfaces.Repositories
{
    public interface IShipRepository : IGenericRepository<Ship>
    {
        Task<List<Ship>> GetShipsByGame(int gameId);
        Task<List<Ship>> GetShipsByPlayer(int gameId, string playerId);
        Task<bool> AllShipsPlaced(int gameId, string playerId);
    }
}