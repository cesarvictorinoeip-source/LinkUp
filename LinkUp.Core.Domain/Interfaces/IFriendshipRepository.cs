using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;

namespace LinkUp.Core.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task<List<Friendship>> GetAcceptedFriendships(string userId);
        Task<bool> HasPendingRequest(string userOneId, string userTwoId);
        Task<List<Friendship>> GetPendingReceived(string userId);
        Task<List<Friendship>> GetSentRequests(string userId);
    }
}