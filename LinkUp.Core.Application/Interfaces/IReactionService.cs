using LinkUp.Core.Application.Dtos.Battleship.Reaction;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IReactionService
    {
        Task<ReactionDto?> GetUserReactionToPostAsync(string userId, int postId);
    }
}