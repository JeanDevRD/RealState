using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Domain.Entities;



namespace RealState.Core.Application.DTOs.PropertyUnit
{
    public class PropertyUnitDto : CommonDto<int>
    {
        public required string IdAgent { get; set; } 

        public required int PropertyTypeId { get; set; } 
        public PropertyTypeDto? PropertyType { get; set; }

        public required int SaleTypeId { get; set; } 
        public SaleType? SaleType { get; set; }

        public ICollection<ImprovementTypeDto>? ImprovementTypes { get; set; } 

        public required string CodeProperty { get; set; } 

        public required decimal Price { get; set; } 
        public required string Description { get; set; } 
        public required double SizeM { get; set; } 
        public required int Bedrooms { get; set; } 
        public required int Bathrooms { get; set; } 
        public required List<string> Images { get; set; }
        public required int StateProperty { get; set; } 

        public ICollection<ChatDto>? Chats { get; set; }
        public ICollection<PropertyOfferDto>? PropertyOffers { get; set; }
    }
}
