using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyType;

namespace RealState.Core.Application.Interfaces
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeDto>
    {
        Task<ResultDto<List<PropertyTypeDto>>> GetAllPropertyType();
        Task<List<PropertyTypeDto>> GetAllWithInclude();
    }
}