using RealState.Core.Application.ViewModels.Common;

namespace RealState.Core.Application.ViewModels.ImprovementType
{
    public class ImprovementTypeViewModel : CommonViewModel<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}
