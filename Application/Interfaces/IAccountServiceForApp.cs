using RealState.Core.Application.DTOs.User;

namespace RealState.Core.Application.Interfaces
{
    public interface IAccountServiceForApp : IBaseAccountService
    {
        Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto);
        Task SignOutAsync();
    }
}
