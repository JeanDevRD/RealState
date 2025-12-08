namespace RealState.Core.Application.ViewModels.PropertyUnit
{
    public class PropertyCardViewModel
    {
        public int Id { get; set; }
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
