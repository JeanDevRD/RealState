using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.Agent.Queries.GetAll
{
    /// <summary>
    /// listar todos los agentes
    /// </summary>
    public class GetAllAgentQuery : IRequest<IList<AgentDto>>
    {
    }

    public class GetAllAgentQueryHandler : IRequestHandler<GetAllAgentQuery, IList<AgentDto>>
    {
        private readonly IAccountServiceForApi _UserForApi;
        private readonly IMapper _mapper;
        private readonly IPropertyUnitRepository _propertyUnitRepo;


        public GetAllAgentQueryHandler(IMapper mapper, IAccountServiceForApi UserForApi, IPropertyUnitRepository propertyUnitRepo)
        {
            _mapper = mapper;
            _UserForApi = UserForApi;
            _propertyUnitRepo = propertyUnitRepo;
        }

        public async Task<IList<AgentDto>> Handle(GetAllAgentQuery query, CancellationToken cancellationToken)
        {

            var agents = await _UserForApi.GetAllUsersByRole(UserRole.Agent.ToString());

            var agentDtos = new List<AgentDto>();
            if (!agents.Any())
            {
                return agentDtos;
            }

            foreach (var agent in agents)
            {
                var propertyCount = await _propertyUnitRepo
                    .GetAllQueryAsync()
                    .Where(p => p!.IdAgent == agent.Id)
                    .CountAsync(cancellationToken);

                var agentDto = new AgentDto
                {
                    Id = agent.Id,
                    Name = agent.FirstName,
                    LastName = agent.LastName,
                    Email = agent.Email,
                    IsActive = agent.IsActive,
                    TotalProperties = propertyCount
                };

                agentDtos.Add(agentDto);
            }
            return agentDtos;
        }


    }
}
