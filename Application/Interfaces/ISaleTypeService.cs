using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.SaleType;

namespace RealState.Core.Application.Interfaces
{
    public interface ISaleTypeService : IGenericService<SaleTypeDto>
    {
        Task<ResultDto<List<SaleTypeDto>>> GetAllSaleType();
    }
}