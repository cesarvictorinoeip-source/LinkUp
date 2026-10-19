using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;

namespace LinkUp.Core.Application.Interfaces.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<List<Game>> GetActiveGames(string userId);
        Task<List<Game>> GetGameHistory(string userId);
        Task<bool> HasActiveGame(string player1Id, string player2Id);
        Task<Game?> GetGameWithDetails(int gameId);
    }
}