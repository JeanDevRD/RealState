using AutoMapper;
using RealState.Core.Application.DTOs.UserFavoritePropertyUnit;
using RealState.Core.Application.ViewModels.UserFavoritePropertyUnit;
namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class UserFavoritePropertyMappingProfile : Profile
    {
        public UserFavoritePropertyMappingProfile()
        {
            CreateMap<UserFavoritePropertyUnitDto, UserFavoritePropertyUnitViewModel>()
                .ForMember(a => a.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(a => a.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
                .ForMember(a => a.IdClient, opt => opt.MapFrom(src => src.IdClient))
                .ReverseMap();


        }
    }
}
