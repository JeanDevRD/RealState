using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class AgentService : IAgentService
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
                        IsActive = agent.IsActive,
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
            if (agent == null)
            {
                return false;
            }

            agent.IsActive = !agent.IsActive;
            await _UserforApp.SetActivated(agent);
            return true;
        }

        public async Task<bool> DeleteAgentAsync(string agentId)
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

        #region List active agents (alfabético)

        public async Task<ResultDto<List<AgentCardDto>>> GetAllActiveAgentsAsync()
        {
            var result = new ResultDto<List<AgentCardDto>>()
            {
                Data = new List<AgentCardDto>(),
                Message = new List<string>()
            };

            try
            {
                var agents = await _UserforApp.GetAllUsersByRole(UserRole.Agent.ToString());

                agents = agents.Where(a => a.IsActive).ToList();

                if (!agents.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron agentes activos");
                    return result;
                }

                result.Data = agents.Select(a => new AgentCardDto
                {
                    Id = a.Id,
                    Name = a.FirstName,
                    LastName = a.LastName,
                    PhotoUrl = a.PhotoUrl
                }).OrderBy(a => a.Name).ToList();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Search agents by name

        public async Task<ResultDto<List<AgentCardDto>>> SearchAgentsByNameAsync(string searchTerm)
        {
            var result = new ResultDto<List<AgentCardDto>>()
            {
                Data = new List<AgentCardDto>(),
                Message = new List<string>()
            };

            try
            {
                var agents = await _UserforApp.GetAllUsersByRole(UserRole.Agent.ToString());

                agents = agents.Where(a => a.IsActive && (a.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || a.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

                if (!agents.Any())
                {
                    result.IsError = true;
                    result.Message.Add($"Agentes encontrados: {agents.Count}");
                }

                result.Data = agents.Select(a => new AgentCardDto
                {
                    Id = a.Id,
                    Name = a.FirstName,
                    LastName = a.LastName,
                    PhotoUrl = a.PhotoUrl,
                }).OrderBy(a => a.Name).ToList();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Get agent available properties
        public async Task<ResultDto<List<PropertyCardDto>>> GetAgentAvailablePropertiesAsync(string agentId)
        {
            var result = new ResultDto<List<PropertyCardDto>>
            {
                Data = new List<PropertyCardDto>(),
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string> { "PropertyType", "SaleType" };

                var properties = await _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes)
                    .Where(p => p!.IdAgent == agentId && p.StateProperty == (int)StateProperty.Available)
                    .OrderByDescending(p => p!.Id)
                    .ToListAsync();

                if (!properties.Any())
                {
                    result.IsError = true;
                    result.Message.Add("Este agente no tiene propiedades disponibles");
                    return result;
                }

                result.Data = properties.Select(p => new PropertyCardDto
                {
                    Id = p!.Id,
                    PropertyTypeName = p.PropertyType?.Name ?? "N/A",
                    FirstImage = p.Images.FirstOrDefault() ?? "",
                    CodeProperty = p.CodeProperty,
                    SaleTypeName = p.SaleType?.Name ?? "N/A",
                    Price = p.Price,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    SizeM = p.SizeM
                }).ToList();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion
    }
}
