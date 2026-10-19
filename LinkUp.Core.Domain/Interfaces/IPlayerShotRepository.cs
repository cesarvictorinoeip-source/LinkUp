using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;

namespace LinkUp.Core.Application.Interfaces.Repositories
{
    public interface IPlayerShotRepository : IGenericRepository<PlayerShot>
    {
        Task<List<PlayerShot>> GetShotsByGame(int gameId);
        Task<List<PlayerShot>> GetShotsByPlayer(int gameId, string playerId);
        Task<bool> CellAlreadyAttacked(int gameId, string playerId, int row, int col);
    }
}