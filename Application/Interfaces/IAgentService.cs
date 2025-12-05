using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.Interfaces
{
    public interface IAgentService
    {
        Task<bool> ChangeStatusAgentAsync(string agentId);
        Task<bool> DeleteAgentAsync(string agentId);
        Task<ResultDto<List<PropertyCardDto>>> GetAgentAvailablePropertiesAsync(string agentId);
        Task<ResultDto<List<AgentCardDto>>> GetAllActiveAgentsAsync();
        Task<ResultDto<List<AgentDto>>> GetAllAgentsAsync();
        Task<int> GetTotalAgentsForAppAsync();
        Task<ResultDto<List<AgentCardDto>>> SearchAgentsByNameAsync(string searchTerm);
    }
}