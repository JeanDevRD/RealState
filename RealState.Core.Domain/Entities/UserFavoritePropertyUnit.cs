using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class UserFavoritePropertyUnit : CommonEntity<int>
    {
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
    }
}
