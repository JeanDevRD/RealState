using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.DTOs.PropertyOffer
{
    public class PropertyOfferWithPropertyDetails : CommonDto<int>
    {
        public required string IdClient { get; set; }
        public required string NameClient { get; set; }
        public PropertyUnitDto? Property { get; set; }
        public required DateTime OfferDate { get; set; }
        public required decimal OfferAmount { get; set; }
        public required int OfferStatus { get; set; }
    }
}
