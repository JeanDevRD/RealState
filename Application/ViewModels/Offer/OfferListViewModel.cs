namespace RealState.Core.Application.ViewModels.Offer
{
    public class OfferListViewModel
    {
        public string ClientId { get; set; } = "";
        public string ClientName { get; set; } = "";
        public int PropertyId { get; set; }
        public int TotalOffers { get; set; }
        public bool HasPendingOffer { get; set; }
    }
}
