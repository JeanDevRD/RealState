using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.ViewModels.PropertyType
{
    public class PropertyTypeViewModel : CommonDto<int> 
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }

        public int? CountProperty { get; set; } = 0;

    }
}
