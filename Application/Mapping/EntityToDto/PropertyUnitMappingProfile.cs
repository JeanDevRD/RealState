using AutoMapper;
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

            #region PropertyUnit to PropertyCardDto
            CreateMap<PropertyUnit, PropertyCardDto>()
            .ForMember(dest => dest.PropertyTypeName,
                opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : "N/A"))
            .ForMember(dest => dest.FirstImage,
                opt => opt.MapFrom(src => src.Images.FirstOrDefault() ?? ""))
            .ForMember(dest => dest.SaleTypeName,
                opt => opt.MapFrom(src => src.SaleType != null ? src.SaleType.Name : "N/A"))
            .ReverseMap();
            #endregion


            #region PropertyUnit to PropertyDetailsDto
            CreateMap<PropertyUnit, PropertyDetailsDto>()
              .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : "N/A"))
              .ForMember(dest => dest.SalesName, opt => opt.MapFrom(src => src.SaleType != null ? src.SaleType.Name : "N/A"))
              .ForMember(dest => dest.ImprovementTypesNames, opt => opt.MapFrom(src =>
                  src.ImprovementTypes != null ? src.ImprovementTypes.Select(i => i.Name).ToList() : new List<string>()))
              .ForMember(dest => dest.Chats, opt => opt.Ignore())
              .ForMember(dest => dest.ClientWithOffer, opt => opt.Ignore());
            #endregion

        }
    }
}
