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

            CreateMap<PropertyUnitDto, PropertyUnitViewModel>()
                .ForMember(dest => dest.FirstImage, opt => opt.MapFrom(src => src.Images.FirstOrDefault()))
                .ReverseMap();

            CreateMap<PropertyCardDto, PropertyUnitViewModel>()
                .ReverseMap();

        }
    }
}
