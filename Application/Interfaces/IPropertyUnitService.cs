using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyUnit;

namespace RealState.Core.Application.Interfaces
{
    public interface IPropertyUnitService : IGenericService<PropertyUnitDto>
    {
        Task<ResultDto<List<PropertyCardDto>>> FilterPropertiesAsync(PropertyFilterDto filter);
        Task<string> GenerateUniquePropertyCodeAsync();
        Task<ResultDto<List<PropertyCardDto>>> GetAllAvailablePropertiesAsync();
        Task<ResultDto<List<PropertyUnitDto>>> GetAllPropertyUnitsByAgent(string idAgent, bool onlyAvailable = false);
        Task<List<PropertyUnitDto>> GetAllWithInclude();
        Task<ResultDto<PropertyCardDto>> GetPropertyByCodeAsync(string code);
        Task<ResultDto<PropertyDetailsDto>> GetPropertyDetailByAgent(int idProperty);
        Task<ResultDto<PropertyDetailHomeDto>> GetPropertyDetailForHomeAsync(int id);
        Task<int> TotalPropertyUnitsAsync();
    }
}