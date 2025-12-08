using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.Agent.Queries.GetById
{
    /// <summary>
    /// Parametros para ver un agente especifico
    /// </summary>
    public class GetByIdAgentQuery : IRequest<AgentDto>
    {
        [SwaggerParameter(Description = "Id del agente")]
        public required string Id { get; set; }
    }

    public class GetByIdAgentQueryHandler : IRequestHandler<GetByIdAgentQuery, AgentDto>
    {
        private readonly IAccountServiceForApi _UserForApi;
        private readonly IMapper _mapper;
        private readonly IPropertyUnitRepository _propertyUnitRepo;

        public GetByIdAgentQueryHandler(IMapper mapper, IAccountServiceForApi UserForApi, IPropertyUnitRepository propertyUnitRepo)
        {
            _mapper = mapper;
            _UserForApi = UserForApi;
            _propertyUnitRepo = propertyUnitRepo;
        }


        public async Task<AgentDto> Handle(GetByIdAgentQuery query, CancellationToken cancellationToken)
        {

            var agent = await _UserForApi.GetUserById(query.Id);

            if (agent == null)
            {
                return null!;
            }

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

            return agentDto;
        }

        
    }
}
