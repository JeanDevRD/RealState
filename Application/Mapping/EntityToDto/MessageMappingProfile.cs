using AutoMapper;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}
