using AutoMapper;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.ViewModels.Message;
namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class MessageViewModelMappingProfile : Profile
    {
        public MessageViewModelMappingProfile() 
        {
            CreateMap<MessageDto, MessageViewModel>()
                   .ReverseMap();
        }
    }
}
