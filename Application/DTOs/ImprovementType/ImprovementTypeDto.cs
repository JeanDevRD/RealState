using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.DTOs.ImprovementType
{
    public class ImprovementTypeDto : CommonDto<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

}
}
