using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Entities;
namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class PropertyUnitMappingProfile : Profile
    {
        public PropertyUnitMappingProfile()
        {
            CreateMap<PropertyUnit, PropertyUnitDto>()
                .ReverseMap();

            CreateMap<PropertyUnit, PropertyCardDto>()
            .ForMember(dest => dest.PropertyTypeName,
                opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : "N/A"))
            .ForMember(dest => dest.FirstImage,
                opt => opt.MapFrom(src => src.Images.FirstOrDefault() ?? ""))
            .ForMember(dest => dest.SaleTypeName,
                opt => opt.MapFrom(src => src.SaleType != null ? src.SaleType.Name : "N/A"))
            .ReverseMap();
        }
    }
}
