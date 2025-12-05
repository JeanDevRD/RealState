using RealState.Core.Application.DTOs.Common;

namespace RealState.Core.Application.ViewModels.SalesType
{
    public class SaleTypeViewModel : CommonDto<int> 
    {
        public required string Name { get; set; } 
        public required string Description { get; set; }

        public int? CountProperty { get; set; } = 0;

    }
}
