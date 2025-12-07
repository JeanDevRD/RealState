using RealState.Core.Application.DTOs.PropertyUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.PropertyOffer
{
    public class PropertyOfferViewModel
    {
        public required string IdClient { get; set; }
        public required int IdProperty { get; set; }
        public PropertyUnitDto? Property { get; set; }
        public required DateTime OfferDate { get; set; }
        public required decimal OfferAmount { get; set; }
        public required int OfferStatus { get; set; }
    }
}
