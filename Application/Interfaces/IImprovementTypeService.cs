using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.ImprovementType;

namespace RealState.Core.Application.Interfaces
{
    public interface IImprovementTypeService : IGenericService<ImprovementTypeDto>
    {
        Task<ResultDto<List<ImprovementTypeDto>>> GetAllImprovementTypes();
        Task<List<ImprovementTypeDto>> GetAllWithInclude();
    }
}