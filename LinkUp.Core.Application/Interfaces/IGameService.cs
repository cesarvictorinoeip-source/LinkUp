using LinkUp.Core.Application.Dtos.Battleship.Game;
using LinkUp.Core.Application.Dtos.Battleship.Ship;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IGameService
    {
        Task<GameDto?> GetByIdAsync(int id);
        Task<List<GameDto>> GetActiveGames(string userId);
        Task<List<GameDto>> GetGameHistory(string userId);
        Task<GameDto> CreateGameAsync(string player1Id, string player2Id);
        Task<bool> PlaceShipAsync(int gameId, string playerId, ShipDto ship);
        Task<bool> AttackAsync(int gameId, string attackerId, string coordinate);
        Task SurrenderAsync(int gameId, string playerId);
        Task<bool> BothPlayersReadyAsync(int gameId);
        Task CheckAbandonedGamesAsync();
    }
}