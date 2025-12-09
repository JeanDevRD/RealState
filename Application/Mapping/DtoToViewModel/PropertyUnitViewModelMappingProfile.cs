using AutoMapper;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class PropertyUnitViewModelMappingProfile : Profile
    {
        public PropertyUnitViewModelMappingProfile()
        {
            CreateMap<SavePropertyViewModel, PropertyUnitDto>()
           .ForMember(d => d.Id, opt => opt.Ignore())
           .ForMember(d => d.IdAgent, opt => opt.Ignore())
           .ForMember(d => d.CodeProperty, opt => opt.Ignore())
           .ForMember(d => d.Images, opt => opt.MapFrom(src => new List<string>()))
           .ForMember(d => d.StateProperty, opt => opt.MapFrom(src => (int)StateProperty.Available));

            CreateMap<PropertyDetailsDto, PropertyDetailViewModel>()
                .ReverseMap();

            CreateMap<PropertyFilterDto, PropertyFilterViewModel>()
                .ReverseMap();

            CreateMap<PropertyCardDto, PropertyCardViewModel>().
                ReverseMap();

            CreateMap<PropertyDetailHomeDto, PropertyDetailHomeViewModel>()
                .ReverseMap();

            CreateMap<PropertyUnit, PropertyDetailHomeDto>()
                .ForMember(dest => dest.PropertyTypeName,
                           opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
                .ForMember(dest => dest.SaleTypeName,
                           opt => opt.MapFrom(src => src.SaleType != null ? src.SaleType.Name : string.Empty))
                .ForMember(dest => dest.ImprovementNames,
                           opt => opt.MapFrom(src => src.ImprovementTypes != null
                                                    ? src.ImprovementTypes.Select(i => i.Name).ToList()
                                                    : new List<string>()));
        }
    }
}
