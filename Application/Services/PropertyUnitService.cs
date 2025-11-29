using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class PropertyUnitService 
    {
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        public PropertyUnitService(IPropertyUnitRepository propertyUnitRepo)
        {
            _propertyUnitRepo = propertyUnitRepo;
        }

        #region Property Unit Counting by Admin

        public async Task<int> TotalPropertyUnitsAsync()
        {
            var propertyUnits = await _propertyUnitRepo.GetAllListAsync();
            return propertyUnits.Count();
        }

        #endregion
    }
}
