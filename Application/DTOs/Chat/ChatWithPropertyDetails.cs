using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.DTOs.Chat
{
    public class ChatWithPropertyDetails : CommonDto<int>
    {
        public required string NameClient { get; set; }
        public required string IdClient { get; set; }
    }
}
