using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.DTOs.PropertyUnit;


namespace RealState.Core.Application.ViewModels.Chat
{
    public class ChatViewModel
    {
        public required int Id { get; set; }
        public required string IdClient { get; set; }
        public required string IdAgent { get; set; }
        public required int IdProperty { get; set; }
        public PropertyUnitDto? Property { get; set; }
        public ICollection<MessageDto>? Messages { get; set; }
    }
}
