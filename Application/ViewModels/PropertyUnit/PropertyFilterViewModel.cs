namespace RealState.Core.Application.ViewModels.PropertyUnit
{
    public class PropertyFilterViewModel
    {
        public int? PropertyTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
    }
}
