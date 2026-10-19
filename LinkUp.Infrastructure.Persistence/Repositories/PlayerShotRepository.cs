using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class PlayerShotRepository : GenericRepository<PlayerShot>, IPlayerShotRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerShotRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<PlayerShot> GetAllQuery()
        {
            return _context.Shots.AsQueryable();
        }

        public override IQueryable<PlayerShot> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Shots.AsQueryable();
            foreach (var prop in properties)
                query = query.Include(prop);
            return query;
        }

        // ✅ Todos los disparos de una partida
        public async Task<List<PlayerShot>> GetShotsByGame(int gameId)
        {
            return await _context.Shots
                .Where(s => s.GameId == gameId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }

        // ✅ Disparos de un jugador en una partida
        public async Task<List<PlayerShot>> GetShotsByPlayer(int gameId, string playerId)
        {
            return await _context.Shots
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }

        // ✅ Verifica si una celda ya fue atacada
        public async Task<bool> CellAlreadyAttacked(int gameId, string playerId, int row, int col)
        {
            return await _context.Shots
                .AnyAsync(s => s.GameId == gameId
                            && s.PlayerId == playerId
                            && s.Row == row
                            && s.Column == col);
        }
    }
}