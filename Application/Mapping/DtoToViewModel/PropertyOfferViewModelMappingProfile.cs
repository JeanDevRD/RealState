using AutoMapper;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.ViewModels.PropertyOffer;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class PropertyOfferViewModelMappingProfile : Profile
    {
        public PropertyOfferViewModelMappingProfile()
        {
            CreateMap<SavePropertyOfferViewModel, PropertyOfferDto>()
                .ForMember(d => d.OfferStatus, opt => opt.Ignore())
                .ForMember(d => d.Property, opt => opt.Ignore())
                .ForMember(d => d.IdClient, opt => opt.Ignore())
                .ForMember(d => d.IdProperty, opt => opt.Ignore())
                .ForMember(d => d.OfferDate, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<PropertyOfferDto, PropertyOfferViewModel>()
                .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property));
        }
    }
}
