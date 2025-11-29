using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Interfaces;
using RealState.Core.Domain.Common.Enums;

namespace RealState.Core.Application.Services
{
    public class AgentService
    {
        private readonly IAccountServiceForApp _UserforApp;
        private readonly IPropertyUnitRepository _propertyUnitRepo;

        public AgentService(IAccountServiceForApp userforApp, IPropertyUnitRepository propertyUnitRepo)
        {
            _UserforApp = userforApp;
            _propertyUnitRepo = propertyUnitRepo;
        }

        #region Agent Counting by Admin

        public async Task<int> GetTotalAgentsForAppAsync()
        {
            var agents = await _UserforApp.GetAllUsersByRole(UserRole.Agent.ToString());
            return agents.Count();
        }

        #endregion

        #region List Agent by Admin

        public async Task<ResultDto<List<AgentDto>>> GetAllAgentsAsync()
        {
            var result = new ResultDto<List<AgentDto>>()
            {
                Data = new List<AgentDto>(),
                Message = new List<string>()
            };

            try
            {
                var agents = await _UserforApp.GetAllUsersByRole(UserRole.Agent.ToString());

                if (!agents.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron agentes");
                    return result;
                }

                foreach (var agent in agents)
                {
                    var propertyCount = await _propertyUnitRepo.GetAllQueryAsync().Where(p => p!.IdAgent == agent.Id).CountAsync(); 
                    
                    var agentDto = new AgentDto
                    {
                        Id = agent.Id,
                        Name = agent.FirstName,
                        LastName = agent.LastName,
                        Email = agent.Email,
                        TotalProperties = propertyCount
                    };
                    result.Data.Add(agentDto);
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Activate or deactivate and delete by Admin

        public async Task<bool> ChangeStatusAgentAsync(string agentId) 
        { 
            var agent = await _UserforApp.GetUserById(agentId);
            if(agent == null)
            {
                return false;
            }

            agent.IsActive = !agent.IsActive;
            await _UserforApp.SetActivated(agent);
            return true;
        }

        public async Task<bool> DeleteStatusAgentAsync(string agentId)
        {
            var agent = await _UserforApp.GetUserById(agentId);
            if (agent == null)
            {
                return false;
            }
            await _UserforApp.DeleteAsync(agentId);
            return true;
        }

        #endregion
    }
}
