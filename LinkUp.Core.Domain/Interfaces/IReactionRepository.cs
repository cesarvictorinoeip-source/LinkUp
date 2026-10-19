using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;

namespace LinkUp.Core.Application.Interfaces.Repositories
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        Task<Reaction?> GetUserReaction(int postId, string userId);
        Task<List<Reaction>> GetReactionsByPost(int postId);
    }
}