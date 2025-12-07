using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.PropertyOffer
{
    public class SavePropertyOfferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "EL monto es requerido")]
        public required decimal OfferAmount { get; set; }
    }
}
