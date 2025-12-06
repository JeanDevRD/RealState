

namespace RealState.Core.Application.ViewModels.Offer
{
    public class OfferDetailViewModel
    {
        public int Id { get; set; }
        public DateTime OfferDate { get; set; }
        public decimal OfferAmount { get; set; }
        public int OfferStatus { get; set; }
        public string StatusText => OfferStatus switch
        {
            0 => "Pendiente",
            1 => "Aceptada",
            2 => "Rechazada",
            _ => "Desconocido"
        };
        public bool IsPending => OfferStatus == 0;
    }
}
