using AutoMapper;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.ViewModels.Admin;
using RealState.Core.Application.ViewModels.Agent;
using RealState.Core.Application.ViewModels.Developer;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class AdminViewModelMappingProfile : Profile
    {
        public AdminViewModelMappingProfile()
        {
            CreateMap<AdminDto, AdminViewModel>()
                .ReverseMap();

            
        }
    }
}
