using RealState.Core.Application.DTOs.PropertyOffer;

namespace RealState.Core.Application.DTOs.User
{
    public class ClientWithPropertyOffer
    {
        public required string NameClient { get; set; }
        public required List<PropertyOfferWithPropertyDetails> PropertyOffers { get; set; } = new();
    }
}
