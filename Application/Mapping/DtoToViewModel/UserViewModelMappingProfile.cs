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
            .ForMember(d => d.ExistingPhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl)) 
            .ForMember(d => d.Photo, opt => opt.Ignore())
            .ForMember(d => d.Password, opt => opt.Ignore())
            .ForMember(d => d.ConfirmPassword, opt => opt.Ignore());

            CreateMap<SaveUserViewModel, SaveUserDto>()
                .ReverseMap();

            CreateMap<EditUserViewModel, SaveUserDto>()
                            .ForMember(d => d.PhotoUrl, opt => opt.Ignore())
                            .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password ?? ""))
                            .ForMember(d => d.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword ?? ""));

            CreateMap<SaveUserDto, EditUserViewModel>()
                .ForMember(d => d.ExistingPhotoUrl, opt => opt.Ignore())
                .ForMember(d => d.Photo, opt => opt.Ignore());

            CreateMap<UserDto, UserViewModel>()
                .ReverseMap();

        }
    }
}
