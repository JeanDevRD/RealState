using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.PropertyUnit
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public double SizeM { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public List<int> ImprovementTypeIds { get; set; } = new();
        public List<string>? ExistingImages { get; set; }
    }
}
