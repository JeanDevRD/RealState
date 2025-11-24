using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class UserFavoriteProperty : CommonEntity<int>
    {
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
    }
}
