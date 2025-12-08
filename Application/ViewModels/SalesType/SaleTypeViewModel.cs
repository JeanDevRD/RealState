using RealState.Core.Application.ViewModels.Common;

namespace RealState.Core.Application.ViewModels.SalesType
{
    public class SaleTypeViewModel : CommonViewModel<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public int? CountProperty { get; set; } = 0;

    }
}
