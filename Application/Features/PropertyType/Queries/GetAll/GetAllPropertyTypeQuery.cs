using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.PropertyType.Commands.CreatePropertyType
{
    public class GetAllPropertyTypeQuery : IRequest<IList<PropertyTypeDto>>
    {
    }

    public class GetAllPropertyTypeQueryHandler : IRequestHandler<GetAllPropertyTypeQuery, IList<PropertyTypeDto>>
    {
        private readonly IPropertyTypeRepository _repo;
        private readonly IPropertyUnitRepository _repoProperty;
        private readonly IMapper _mapper;

        public GetAllPropertyTypeQueryHandler(IPropertyTypeRepository repo, IMapper mapper, IPropertyUnitRepository repoProperty)
        {
            _repo = repo;
            _mapper = mapper;
            _repoProperty = repoProperty;
        }

        public async Task<IList<PropertyTypeDto>> Handle(GetAllPropertyTypeQuery query, CancellationToken cancellationToken)
        {

            var propertyTypes = await _repo.GetAllListAsync();
            if (!propertyTypes.Any())
            {
                return [];
            }

            var propertyTypeList = _mapper.Map<List<PropertyTypeDto>>(propertyTypes);

            foreach (var dto in propertyTypeList)
            {
                var property = await _repoProperty.GetAllQueryAsync().Where(p => p.PropertyTypeId == dto.Id).CountAsync();
                dto.CountProperty = property;
            }
            return propertyTypeList;
        }


    }
}
