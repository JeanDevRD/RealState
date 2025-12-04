using RealState.Core.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.DTOs.PropertyUnit
{
     public class PropertyCardDto : CommonDto<int>
    {
        public required string PropertyTypeName { get; set; } 
        public required string FirstImage { get; set; }  
        public required string CodeProperty { get; set; }  
        public required string SaleTypeName { get; set; } 
        public required decimal Price { get; set; }  
        public required int Bedrooms { get; set; }  
        public required int Bathrooms { get; set; }  
        public required double SizeM { get; set; }
    }
}
