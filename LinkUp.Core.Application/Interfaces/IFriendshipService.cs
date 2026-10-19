using LinkUp.Core.Application.Dtos.Battleship.Friendship;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IFriendshipService : IGenericService<Friendship, FriendshipDto>
    {
        Task<List<FriendshipDto>> GetUserFriends(string userId);
        Task<List<string>> GetFriendIdsAsync(string userId);
        Task AcceptFriendRequest(int friendshipId);
        Task RejectFriendRequest(int friendshipId);
        Task DeleteFriendship(int friendshipId);
        Task<bool> SendFriendRequest(string senderId, string receiverId);
        Task<List<FriendshipDto>> GetPendingRequestsReceived(string userId);
        Task<List<FriendshipDto>> GetPendingRequestsSent(string userId);
        Task<bool> AlreadyFriendsOrPending(string userId1, string userId2);
    }
}