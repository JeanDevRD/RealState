using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.PropertyType.Commands.CreatePropertyType
{
    public class GetByIdPropertyTypeQuery : IRequest<PropertyTypeDto>
    {
        public required int Id { get; set; }
    }

    public class GetByIdPropertyTypeQueryHandler : IRequestHandler<GetByIdPropertyTypeQuery, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _repo;
        private readonly IPropertyUnitRepository _repoProperty;
        private readonly IMapper _mapper;

        public GetByIdPropertyTypeQueryHandler(IPropertyTypeRepository repo, IMapper mapper, IPropertyUnitRepository repoProperty)
        {
            _repo = repo;
            _mapper = mapper;
            _repoProperty = repoProperty;
        }

        public async Task<PropertyTypeDto> Handle(GetByIdPropertyTypeQuery query, CancellationToken cancellationToken)
        {

            var propertyTypes = await _repo.GetByIdAsync(query.Id);
            if (propertyTypes == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PROPIEDAD");

            var propertyType = _mapper.Map<PropertyTypeDto>(propertyTypes);
            var property = await _repoProperty.GetAllQueryAsync().Where(p => p.PropertyTypeId == propertyType.Id).CountAsync();
            propertyType.CountProperty = property;
            return propertyType;
        }

        
    }
}
