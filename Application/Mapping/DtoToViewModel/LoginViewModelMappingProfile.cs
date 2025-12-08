using AutoMapper;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.ViewModels.User;
namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class LoginViewModelMappingProfile : Profile
    {
        public LoginViewModelMappingProfile()
        {
            CreateMap<LoginViewModel, LoginDto>()
                .ReverseMap();
        }
    }
}
