using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.Interfaces
{
    public interface IAdminService
    {
        Task<bool> ChangeStatusAdminAsync(string adminId);
        Task<ResultDto<List<AdminDto>>> GetAllAdminAsync();
    }
}