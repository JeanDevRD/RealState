using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class ChatService : GenericService<Chat, ChatDto>, IChatService
    {
        public readonly IChatRepository _chatRepo;
        public readonly IMapper _mapper;
        public ChatService(IChatRepository chatRepo, IMapper mapper) : base(chatRepo, mapper)
        {
            _chatRepo = chatRepo;
            _mapper = mapper;
        }

        public async Task<List<ChatDto>> GetAllWithInclude()
        {
            try
            {
                var improvementTypes = await _chatRepo.GetAllListIncluide(["Messages"]);

                if (improvementTypes == null)
                {
                    return new List<ChatDto>();
                }
                return _mapper.Map<List<ChatDto>>(improvementTypes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving chat data with included data: " + ex.Message);
            }
        }
        public async Task<ChatDto> GetConversation(string propertyId, string clientId)
        {
            try
            {
                var chat = await GetAllWithInclude();
                chat = chat.Where(c => c.IdProperty.ToString() == propertyId && c.IdClient == clientId).ToList();


                if (chat == null)
                {
                    return null!;

                }

                return _mapper.Map<ChatDto>(chat);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving conversation: " + ex.Message);
            }
        }



    }
}