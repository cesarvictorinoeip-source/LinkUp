using LinkUp.Core.Application.Dtos.Battleship.User;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetById(int id);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<List<UserDto>> GetUserSuggestionsAsync(string currentUserId);
    }
}