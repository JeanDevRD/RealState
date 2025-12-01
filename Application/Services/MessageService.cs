using AutoMapper;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class MessageService : GenericService<Message,MessageDto>
    {
        private readonly IMessageRepository _chatRepo;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository chatRepo, IMapper mapper) : base (chatRepo, mapper)
        {
            _chatRepo = chatRepo;
            _mapper = mapper;   
        }

        public async Task<List<MessageDto>> GetAllMessages()
        {
            try
            {
                var messages = await _chatRepo.GetAllListIncluide(["Chat"]);
                if (messages == null)
                {
                    return new List<MessageDto>();
                }
                return _mapper.Map<List<MessageDto>>(messages);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving messages: " + ex.Message);
            }
        }
    }
}
