using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.Agent.Queries.GetPropertyByAgent
{
    /// <summary>
    /// obtener todas las propiedades de un agente por id
    /// </summary>
    public class GetAllAgentPropertiesByIdQuery : IRequest<IList<PropertyUnitDto>>
    {
        ///<example>1</example>
        [SwaggerParameter(Description = "Id agente")]
        public required string IdAgent { get; set; }
    }

    public class GetAllAgentPropertiesByIdQueryHandler : IRequestHandler<GetAllAgentPropertiesByIdQuery, IList<PropertyUnitDto>>
    {
        private readonly IPropertyUnitRepository _repo;
        private readonly IMapper _mapper;

        public GetAllAgentPropertiesByIdQueryHandler(IPropertyUnitRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IList<PropertyUnitDto>> Handle(GetAllAgentPropertiesByIdQuery query, CancellationToken cancellationToken)
        {

            var property = await _repo.GetAllQueryAsync().Where(a => a!.IdAgent == query.IdAgent).ToListAsync();
            if (!property.Any())
            {
                return [];
            }

            var propertyList = _mapper.Map<List<PropertyUnitDto>>(property);
            return propertyList;
        }

        
    }
}
