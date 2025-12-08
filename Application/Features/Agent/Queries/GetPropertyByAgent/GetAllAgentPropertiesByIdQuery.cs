using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.PropertyUnit.Queries.GetAll
{
    public class GetAllPropertyUnitQuery : IRequest<IList<PropertyUnitDto>>
    {
    }

    public class GetAllPropertyUnitQueryHandler : IRequestHandler<GetAllPropertyUnitQuery, IList<PropertyUnitDto>>
    {
        private readonly IPropertyUnitRepository _repo;
        private readonly IMapper _mapper;

        public GetAllPropertyUnitQueryHandler(IPropertyUnitRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IList<PropertyUnitDto>> Handle(GetAllPropertyUnitQuery query, CancellationToken cancellationToken)
        {

            var property = await _repo.GetAllListAsync();
            if (!property.Any())
            {
                return [];
            }

            var propertyList = _mapper.Map<List<PropertyUnitDto>>(property);
            return propertyList;
        }


    }
}
