using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class ReactionRepository : GenericRepository<Reaction>, IReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Reacción de un usuario en un post específico
        public async Task<Reaction?> GetUserReaction(int postId, string userId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
        }

        // ✅ Todas las reacciones de un post
        public async Task<List<Reaction>> GetReactionsByPost(int postId)
        {
            return await _context.Reactions
                .Where(r => r.PostId == postId)
                .ToListAsync();
        }
    }
}