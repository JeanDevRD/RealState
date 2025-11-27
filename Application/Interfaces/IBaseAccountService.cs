using RealState.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.Interfaces
{
    public interface IBaseAccountService
    {
        Task<RegisterResponseDto> RegisterUser(SaveUserDto saveDto, string? origin, bool? isApi = false);
        Task<EditResponseDto> EditUser(SaveUserDto saveDto, string? origin, bool? isCreated = false, bool? isApi = false);
        Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request, bool? isApi = false);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<UserResponseDto> DeleteAsync(string id);
        Task<UserDto?> GetUserByEmail(string email);
        Task<UserDto?> GetUserById(string id);
        Task<UserDto?> GetUserByUserName(string userName);
        Task<List<UserDto>> GetAllUser();
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<UserDto?> SetActivated(UserDto dto);
    }
}
