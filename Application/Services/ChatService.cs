using AutoMapper;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.Services
{
    public class ChatService : GenericService<Chat, ChatDto>
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
    }
}