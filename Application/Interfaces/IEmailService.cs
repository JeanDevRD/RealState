using RealState.Core.Application.DTOs.Email;

namespace RealState.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto emailRequest);
    }
}