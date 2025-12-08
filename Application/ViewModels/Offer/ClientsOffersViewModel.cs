namespace RealState.Core.Application.ViewModels.Offer
{
    public class ClientsOffersViewModel
    {
        public string ClientId { get; set; } = "";
        public string ClientName { get; set; } = "";
        public int PropertyId { get; set; }
        public string PropertyCode { get; set; } = "";
        public List<OfferDetailViewModel> Offers { get; set; } = new();
    }
}
