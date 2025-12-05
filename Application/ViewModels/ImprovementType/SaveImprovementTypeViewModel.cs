using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.ViewModels.ImprovementType
{
    public class SaveImprovementTypeViewModel : CommonDto<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

}
}
