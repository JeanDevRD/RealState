using RealState.Core.Application.DTOs.User;

namespace RealState.Core.Application.Interfaces
{
    public interface IAccountServiceForApi : IBaseAccountService
    {
        Task<LoginResponseForApiDto> AuthenticateAsync(LoginDto loginDto);
    }
}
