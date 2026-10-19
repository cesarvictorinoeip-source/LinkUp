using LinkUp.Core.Application.Dtos.Battleship.Playershot;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IPlayerShotService : IGenericService<PlayerShot, PlayerShotDto>
    {
        Task<List<PlayerShotDto>> GetShotsByGameAsync(int gameId);
    }
}