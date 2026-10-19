using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FriendshipRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<Friendship> GetAllQuery()
        {
            return _dbContext.Friendships.AsQueryable();
        }

        // ✅ Obtener amigos aceptados de un usuario
        public async Task<List<Friendship>> GetAcceptedFriendships(string userId)
        {
            return await _dbContext.Friendships
                .Where(f => (f.IdUserOne == userId || f.IdUserTwo == userId)
                         && f.IsAccepted)
                .ToListAsync();
        }

        // ✅ Verificar si ya existe una solicitud pendiente entre dos usuarios
        public async Task<bool> HasPendingRequest(string userOneId, string userTwoId)
        {
            return await _dbContext.Friendships
                .AnyAsync(f =>
                    ((f.IdUserOne == userOneId && f.IdUserTwo == userTwoId) ||
                     (f.IdUserOne == userTwoId && f.IdUserTwo == userOneId))
                    && !f.IsAccepted && !f.IsRejected);
        }

        // ✅ Solicitudes recibidas pendientes
        public async Task<List<Friendship>> GetPendingReceived(string userId)
        {
            return await _dbContext.Friendships
                .Where(f => f.IdUserTwo == userId && !f.IsAccepted && !f.IsRejected)
                .ToListAsync();
        }

        // ✅ Solicitudes enviadas
        public async Task<List<Friendship>> GetSentRequests(string userId)
        {
            return await _dbContext.Friendships
                .Where(f => f.IdUserOne == userId)
                .ToListAsync();
        }
    }
}