using RealState.Core.Application.ViewModels.PropertyType;
using RealState.Core.Application.ViewModels.PropertyUnit;

namespace RealState.Core.Application.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<PropertyCardViewModel>? Properties { get; set; }
        public List<PropertyTypeViewModel>? PropertyTypes { get; set; }
        public PropertyFilterViewModel? CurrentFilters { get; set; }
    }
}
