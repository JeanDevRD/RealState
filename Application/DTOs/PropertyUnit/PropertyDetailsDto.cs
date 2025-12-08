using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.User;

namespace RealState.Core.Application.DTOs.PropertyUnit
{
    public class PropertyDetailsDto : CommonDto<int>
    {
        public required string PropertyTypeName { get; set; }
        public required string SalesName { get; set; }
        public required List<string> ImprovementTypesNames { get; set; }
        public required string CodeProperty { get; set; }

        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required double SizeM { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public required List<string> Images { get; set; }
        public required int StateProperty { get; set; }

        public required List<ChatWithPropertyDetails> Chats { get; set; } = new();
        public required List<ClientWithPropertyOffer> ClientWithOffer { get; set; } = new();


    }
}
