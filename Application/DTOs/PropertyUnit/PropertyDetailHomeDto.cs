using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.DTOs.PropertyUnit
{
    public class PropertyDetailHomeDto : CommonDto<int>
    {
        public required string PropertyTypeName { get; set; }
        public required string SaleTypeName { get; set; }
        public required string CodeProperty { get; set; }
        public required decimal Price { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public required double SizeM { get; set; }
        public required string Description { get; set; }
        public required List<string> Images { get; set; }
        public required List<string> ImprovementNames { get; set; }

        public required string AgentName { get; set; }
        public required string AgentPhone { get; set; }
        public required string AgentEmail { get; set; }
        public string? AgentPhoto { get; set; }
    }
}
