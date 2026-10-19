using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Friendship;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class FriendshipService : GenericService<Friendship, FriendshipDto>, IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IMapper _mapper;

        public FriendshipService(
            IFriendshipRepository friendshipRepository,
            IMapper mapper
        ) : base(friendshipRepository, mapper)
        {
            _friendshipRepository = friendshipRepository;
            _mapper = mapper;
        }

        public async Task<List<FriendshipDto>> GetUserFriends(string userId)
        {
            var friendships = await _friendshipRepository.GetAllQueryWithInclude(
                new List<string> { "Sender", "Receiver" })
                .Where(f => (f.IdUserOne == userId || f.IdUserTwo == userId) && f.IsAccepted)
                .ToListAsync();

            return friendships.Select(f => new FriendshipDto
            {
                Id = f.Id,
                IdUserOne = f.IdUserOne,
                IdUserTwo = f.IdUserTwo,
                UserOneName = f.Sender?.UserName ?? f.IdUserOne,
                UserTwoName = f.Receiver?.UserName ?? f.IdUserTwo,
                IsAccepted = f.IsAccepted,
                IsRejected = f.IsRejected,
                CreatedAt = f.CreatedAt
            }).ToList();
        }

        public async Task<List<string>> GetFriendIdsAsync(string userId)
        {
            var friends = await GetUserFriends(userId);
            return friends
                .Select(f => f.IdUserOne == userId ? f.IdUserTwo : f.IdUserOne)
                .ToList();
        }

        public async Task AcceptFriendRequest(int friendshipId)
        {
            var friendship = await _friendshipRepository.GetAllQuery()
                .FirstOrDefaultAsync(f => f.Id == friendshipId);
            if (friendship != null)
            {
                friendship.IsAccepted = true;
                friendship.IsRejected = false;
                await _friendshipRepository.UpdateAsync(friendship, friendship.Id);
            }
        }

        public async Task RejectFriendRequest(int friendshipId)
        {
            var friendship = await _friendshipRepository.GetAllQuery()
                .FirstOrDefaultAsync(f => f.Id == friendshipId);
            if (friendship != null)
            {
                friendship.IsRejected = true;
                friendship.IsAccepted = false;
                await _friendshipRepository.UpdateAsync(friendship, friendship.Id);
            }
        }

        public async Task DeleteFriendship(int friendshipId)
        {
            var friendship = await _friendshipRepository.GetAllQuery()
                .FirstOrDefaultAsync(f => f.Id == friendshipId);
            if (friendship != null)
                await _friendshipRepository.DeleteAsync(friendship);
        }

        public async Task<bool> SendFriendRequest(string senderId, string receiverId)
        {
            var exists = await AlreadyFriendsOrPending(senderId, receiverId);
            if (exists) return false;

            var friendship = new Friendship
            {
                IdUserOne = senderId,
                IdUserTwo = receiverId,
                IsAccepted = false,
                IsRejected = false,
                CreatedAt = DateTime.Now
            };
            await _friendshipRepository.AddAsync(friendship);
            return true;
        }

        public async Task<List<FriendshipDto>> GetPendingRequestsReceived(string userId)
        {
            var friendships = await _friendshipRepository.GetAllQueryWithInclude(
                new List<string> { "Sender", "Receiver" })
                .Where(f => f.IdUserTwo == userId && !f.IsAccepted && !f.IsRejected)
                .ToListAsync();

            return friendships.Select(f => new FriendshipDto
            {
                Id = f.Id,
                IdUserOne = f.IdUserOne,
                IdUserTwo = f.IdUserTwo,
                UserOneName = f.Sender?.UserName ?? f.IdUserOne,
                UserTwoName = f.Receiver?.UserName ?? f.IdUserTwo,
                IsAccepted = f.IsAccepted,
                IsRejected = f.IsRejected,
                CreatedAt = f.CreatedAt
            }).ToList();
        }

        public async Task<List<FriendshipDto>> GetPendingRequestsSent(string userId)
        {
            var friendships = await _friendshipRepository.GetAllQueryWithInclude(
                new List<string> { "Sender", "Receiver" })
                .Where(f => f.IdUserOne == userId && !f.IsAccepted && !f.IsRejected)
                .ToListAsync();

            return friendships.Select(f => new FriendshipDto
            {
                Id = f.Id,
                IdUserOne = f.IdUserOne,
                IdUserTwo = f.IdUserTwo,
                UserOneName = f.Sender?.UserName ?? f.IdUserOne,
                UserTwoName = f.Receiver?.UserName ?? f.IdUserTwo,
                IsAccepted = f.IsAccepted,
                IsRejected = f.IsRejected,
                CreatedAt = f.CreatedAt
            }).ToList();
        }

        public async Task<bool> AlreadyFriendsOrPending(string userId1, string userId2)
        {
            return await _friendshipRepository.GetAllQuery()
                .AnyAsync(f =>
                    ((f.IdUserOne == userId1 && f.IdUserTwo == userId2) ||
                     (f.IdUserOne == userId2 && f.IdUserTwo == userId1)) &&
                    !f.IsRejected);
        }
    }
}