using AutoMapper;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class PropertyOfferMappingProfile : Profile
    {
        public PropertyOfferMappingProfile()
        {
            CreateMap<PropertyOffer, PropertyOfferDto>().ReverseMap();
        }
    }
}
