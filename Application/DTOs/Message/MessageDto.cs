using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.Chat;

namespace RealState.Core.Application.DTOs.Message
{
    public class MessageDto : CommonDto<int>
    {
        public required int IdChat { get; set; }
        public ChatDto? Chat { get; set; }
        public required string SenderId { get; set; }
        public required string Content { get; set; }
        public required DateTime SentAt { get; set; }
    }
}
