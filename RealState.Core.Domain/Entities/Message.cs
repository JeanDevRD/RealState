using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class Message : CommonEntity<int>
    {
        public required int IdChat { get; set; }
        public Chat? Chat { get; set; }

        public required string SenderId { get; set; }
        public required string Content { get; set; }
        public required DateTime SentAt { get; set; }
    }
}
