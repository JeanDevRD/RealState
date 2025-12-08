namespace RealState.Core.Application.ViewModels.PropertyUnit
{
    public class PropertyUnitViewModel
    {
        public int Id { get; set; }
        public string PropertyTypeName { get; set; } = "";
        public string FirstImage { get; set; } = "";
        public string CodeProperty { get; set; } = "";
        public string SaleTypeName { get; set; } = "";
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double SizeM { get; set; }
        public int StateProperty { get; set; }
        public bool IsSold => StateProperty == 1;

    }
}
