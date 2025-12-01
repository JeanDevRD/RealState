using AutoMapper;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class PropertyUnitService : GenericService<PropertyUnit, PropertyUnitDto>
    {
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IMapper _mapper;
        public PropertyUnitService(IPropertyUnitRepository propertyUnitRepo, IMapper mapper) : base(propertyUnitRepo, mapper)
        {
            _propertyUnitRepo = propertyUnitRepo;
            _mapper = mapper;
        }

        public async Task<List<PropertyUnitDto>> GetAllWithInclude()
        {
            try
            {
                var propertyUnits = await _propertyUnitRepo.GetAllListIncluide(["PropertyType", "SaleType", "ImprovementTypes", "Chats", "PropertyOffers"]);
                if (propertyUnits == null)
                {
                    return new List<PropertyUnitDto>();
                }
                return _mapper.Map<List<PropertyUnitDto>>(propertyUnits);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving property units with included data: " + ex.Message);
            }
        }

        #region Property Unit Counting by Admin

        public async Task<int> TotalPropertyUnitsAsync()
        {
            var propertyUnits = await _propertyUnitRepo.GetAllListAsync();
            return propertyUnits.Count();
        }

        #endregion

        #region Propierty Details whith message

     

        #endregion 
    }
}
