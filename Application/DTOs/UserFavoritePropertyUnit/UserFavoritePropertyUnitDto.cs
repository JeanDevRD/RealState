using RealState.Core.Application.DTOs.Common;


namespace RealState.Core.Application.DTOs.UserFavoritePropertyUnit
{
    public class UserFavoritePropertyUnitDto : CommonDto<int>
    {
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
    }
}
