using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<Post> GetAllQuery()
        {
            return _context.Posts.AsQueryable();
        }

        // ✅ Posts de un usuario con comentarios y reacciones
        public async Task<List<Post>> GetPostsByUser(string userId)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // ✅ Posts de varios usuarios (home de amigos)
        public async Task<List<Post>> GetPostsByUsers(List<string> userIds)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .Where(p => userIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}