using AutoMapper;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class PropertyTypeMappingProfile : Profile
    {
        public PropertyTypeMappingProfile()
        {
            CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();
        }   
    }
}
