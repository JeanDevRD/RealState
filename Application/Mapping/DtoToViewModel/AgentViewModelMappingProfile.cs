using AutoMapper;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.ViewModels.Agent;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class AgentViewModelMappingProfile : Profile
    {
        public AgentViewModelMappingProfile()
        {
            CreateMap<AgentCardDto, AgentCardViewModel>()
                .ReverseMap();


        }   
    }
}
