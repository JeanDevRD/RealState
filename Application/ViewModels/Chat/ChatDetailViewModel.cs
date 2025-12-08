using RealState.Core.Application.ViewModels.Message;

namespace RealState.Core.Application.ViewModels.Chat
{
    public class ChatDetailViewModel
    {
        public int ChatId { get; set; }
        public string ClientName { get; set; } = "";
        public int PropertyId { get; set; }
        public string PropertyCode { get; set; } = "";
        public string? CurrentUserId { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new();
    }
}
