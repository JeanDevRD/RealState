using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.ViewModels.PropertyType
{
    public class SavePropertyTypeViewModel : CommonDto<int> 
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }


    }
}
