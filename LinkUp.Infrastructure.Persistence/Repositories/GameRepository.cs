using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<Game> GetAllQuery()
        {
            return _context.Games.AsQueryable();
        }

        // ✅ Partidas activas de un usuario
        public async Task<List<Game>> GetActiveGames(string userId)
        {
            return await _context.Games
                .Include(g => g.Ships)
                .Include(g => g.Shots)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId)
                         && !g.IsFinished)
                .ToListAsync();
        }

        // ✅ Historial de partidas finalizadas
        public async Task<List<Game>> GetGameHistory(string userId)
        {
            return await _context.Games
                .Include(g => g.Shots)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId)
                         && g.IsFinished)
                .OrderByDescending(g => g.FinishedAt)
                .ToListAsync();
        }

        // ✅ Verificar si ya hay partida activa entre dos usuarios
        public async Task<bool> HasActiveGame(string player1Id, string player2Id)
        {
            return await _context.Games
                .AnyAsync(g =>
                    ((g.Player1Id == player1Id && g.Player2Id == player2Id) ||
                     (g.Player1Id == player2Id && g.Player2Id == player1Id))
                    && !g.IsFinished);
        }

        // ✅ Partida completa con barcos y disparos
        public async Task<Game?> GetGameWithDetails(int gameId)
        {
            return await _context.Games
                .Include(g => g.Ships)
                .Include(g => g.Shots)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }
    }
}