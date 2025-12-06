using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
