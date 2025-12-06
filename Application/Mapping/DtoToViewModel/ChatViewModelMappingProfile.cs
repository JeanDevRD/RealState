using AutoMapper;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.ViewModels.Chat;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class ChatViewModelMappingProfile : Profile
    {
        public ChatViewModelMappingProfile()
        {
            CreateMap<ChatDto, ChatViewModel>()
                .ReverseMap();
        }
    }
}
