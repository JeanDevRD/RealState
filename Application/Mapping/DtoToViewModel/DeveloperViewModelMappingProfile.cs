using AutoMapper;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.ViewModels.Agent;
using RealState.Core.Application.ViewModels.Developer;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class DeveloperViewModelMappingProfile : Profile
    {
        public DeveloperViewModelMappingProfile()
        {
            CreateMap<DeveloperDto, DeveloperViewModel>()
                .ReverseMap();

            
        }
    }
}
