using AutoMapper;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile()
        {
            CreateMap<Chat, ChatDto>().ReverseMap();
        }
    }
}
