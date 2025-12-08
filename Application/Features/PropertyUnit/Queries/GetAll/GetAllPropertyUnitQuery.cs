using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.PropertyUnit.Queries.GetAll
{
    public class GetAllAgentPropertiesByIdQuery : IRequest<IList<PropertyUnitDto>>
    {
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
