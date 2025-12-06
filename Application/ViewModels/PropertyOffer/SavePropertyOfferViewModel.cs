using RealState.Core.Application.DTOs.PropertyUnit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.PropertyOffer
{
    public class SavePropertyOfferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "EL monto es requerido")]
        public required decimal OfferAmount { get; set; }
    }
}
