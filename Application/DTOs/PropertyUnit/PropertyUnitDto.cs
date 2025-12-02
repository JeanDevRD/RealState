using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.DTOs.PropertyUnit
{
    public class PropertyUnitDto : CommonDto<int>
    {
        public required string IdAgent { get; set; } 

        public required int PropertyTypeId { get; set; } 

        public required int SaleTypeId { get; set; } 

        public required string CodeProperty { get; set; } 

        public required decimal Price { get; set; } 
        public required string Description { get; set; } 
        public required double SizeM { get; set; } 
        public required int Bedrooms { get; set; } 
        public required int Bathrooms { get; set; } 
        public required List<string> Images { get; set; }
        public required int StateProperty { get; set; } 

    }
}
