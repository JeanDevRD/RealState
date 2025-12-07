using RealState.Core.Application.ViewModels.Message;
using RealState.Core.Application.ViewModels.PropertyUnit;


namespace RealState.Core.Application.ViewModels.Chat
{
    public class ChatViewModel
    {
        public required int Id { get; set; }
        public required string IdClient { get; set; }
        public required string IdAgent { get; set; }
        public required int IdProperty { get; set; }
        public PropertyUnitViewModel? Property { get; set; }
        public ICollection<MessageViewModel>? Messages { get; set; }
    }
}
