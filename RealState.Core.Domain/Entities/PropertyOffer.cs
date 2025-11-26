using RealState.Core.Domain.Common;

namespace RealState.Core.Domain.Entities
{
    public class PropertyOffer : CommonEntity<int> //OfertaDePropiedad
    {
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
        public PropertyUnit? Property { get; set; }
        public required DateTime OfferDate { get; set; }
        public required decimal OfferAmount { get; set; }
        public required int OfferStatus { get; set; }
    }
}
