using LinkUp.Core.Application.Dtos.Battleship.Email;
using LinkUp.Core.Application.Interfaces;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}