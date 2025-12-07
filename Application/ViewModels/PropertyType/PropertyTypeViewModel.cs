using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.ViewModels.Common;

namespace RealState.Core.Application.ViewModels.PropertyType
{
    public class PropertyTypeViewModel : CommonViewModel<int> 
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }

        public int? CountProperty { get; set; } = 0;

    }
}
