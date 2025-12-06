using AutoMapper;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Core.Domain.Common.Enums;

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
        }
    }
}
