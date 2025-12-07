using RealState.Core.Application.DTOs.Chat;

namespace RealState.Core.Application.Interfaces
{
    public interface IChatService : IGenericService<ChatDto>
    {
        Task<List<ChatDto>> GetAllWithInclude();
        Task<ChatDto> GetConversation(int propertyId, string clientId);
    }
}