using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class ShipRepository : GenericRepository<Ship>, IShipRepository
    {
        private readonly ApplicationDbContext _context;

        public ShipRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Todos los barcos de una partida
        public async Task<List<Ship>> GetShipsByGame(int gameId)
        {
            return await _context.Ships
                .Where(s => s.GameId == gameId)
                .ToListAsync();
        }

        // ✅ Barcos de un jugador específico en una partida
        public async Task<List<Ship>> GetShipsByPlayer(int gameId, string playerId)
        {
            return await _context.Ships
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .ToListAsync();
        }

        // ✅ Verifica si el jugador colocó los 5 barcos requeridos
        public async Task<bool> AllShipsPlaced(int gameId, string playerId)
        {
            var count = await _context.Ships
                .CountAsync(s => s.GameId == gameId && s.PlayerId == playerId);
            return count >= 5;
        }
    }
}