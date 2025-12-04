using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.Interfaces
{
    public interface IDeveloperService
    {
        Task<bool> ChangeStatusDeveloperAsync(string developerId);
        Task<ResultDto<List<DeveloperDto>>> GetAllDevelopersAsync();
        Task<int> GetTotalDevelopersForAppAsync();
    }
}