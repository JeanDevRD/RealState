using AutoMapper;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.ViewModels.User;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class UserViewModelMappingProfile : Profile
    {
        public UserViewModelMappingProfile()
        {
            CreateMap<UserDto, EditUserViewModel>()
            .ForMember(d => d.Phone, opt => opt.MapFrom(src => src.Phone ?? ""))
            .ForMember(d => d.Photo, opt => opt.Ignore())
            .ForMember(d => d.Password, opt => opt.Ignore())
            .ForMember(d => d.ConfirmPassword, opt => opt.Ignore());

            CreateMap<SaveUserDto, SaveUserViewModel>()
                .ReverseMap();
        }
    }
}
