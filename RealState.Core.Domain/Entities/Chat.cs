using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class Chat : CommonEntity<int>
    {
        public required string IdClient { get; set; }
        public required string IdAgent { get; set; }
        public required int IdProperty { get; set; }
        public PropertyUnit? Property { get; set; }

        public List<Message> Messages { get; set; } = new();
    }
}
