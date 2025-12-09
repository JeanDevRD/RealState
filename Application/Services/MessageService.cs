using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class MessageService : GenericService<Message, MessageDto>, IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository chatRepo, IMapper mapper) : base(chatRepo, mapper)
        {
            _messageRepo = chatRepo;
            _mapper = mapper;
        }

        public async Task<List<MessageDto>> GetAllMessages()
        {
            try
            {
                var messages = await _messageRepo.GetAllListIncluide(["Chat"]);
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

        public async Task<List<MessageDto>> GetConversation(int chatId) 
        {
            try
            {
                var messages = await _messageRepo.GetAllQueryAsync()
                    .Where(m => m!.IdChat == chatId) 
                    .OrderBy(m => m!.SentAt) 
                    .ToListAsync();

                if (messages == null || !messages.Any())
                {
                    return [];
                }
                return _mapper.Map<List<MessageDto>>(messages);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving conversation by chat ID: " + ex.Message);
            }
        }

    }
}