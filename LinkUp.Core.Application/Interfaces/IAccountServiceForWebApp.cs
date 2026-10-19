using LinkUp.Core.Application.Dtos.Battleship.User;
using LinkUp.Core.Application.ViewModels.User;

namespace LinkUp.Infrastructure.Identity.Services
{
    public interface IAccountServiceForWebApp
    {
        Task<AuthenticationResponse> AuthenticateAsync(LoginViewModel loginDto);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task DeleteUserAsync(string id);
        Task ForgotPasswordAsync(ForgotPasswordRequestViewModel request);
        Task<IEnumerable<AuthenticationResponse>> GetAllUser(bool isActive);
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<SaveUserViewModel?> GetUserByIdAsync(string id);
        Task<string> RegisterUserAsync(SaveUserViewModel vm);
        Task ResetPasswordAsync(ResetPasswordRequestViewModel request);
        Task SignOutAsync();
        Task UpdateUserAsync(SaveUserViewModel vm);
    }
}