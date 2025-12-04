using RealState.Core.Application.DTOs.Message;

namespace RealState.Core.Application.Interfaces
{
    public interface IMessageService : IGenericService<MessageDto>
    {
        Task<List<MessageDto>> GetAllMessages();
        Task<List<MessageDto>> GetConversation(int messageId);
    }
}