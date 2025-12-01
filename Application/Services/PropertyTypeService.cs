using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyType;
<<<<<<< HEAD
using RealState.Core.Application.DTOs.PropertyUnit;
=======
>>>>>>> 7ef5dd215724d2e6d01d1890e4b2c2f0f9e92cad
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
<<<<<<< HEAD
    public class PropertyTypeService : GenericService<PropertyType,PropertyTypeDto>
=======
    public class PropertyTypeService : GenericService<PropertyType, PropertyTypeDto>
>>>>>>> 7ef5dd215724d2e6d01d1890e4b2c2f0f9e92cad
    {
        private readonly IPropertyTypeRepository _propertyTypeRepo;
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IMapper _mapper;

<<<<<<< HEAD
        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepo, IPropertyUnitRepository propertyUnitRepo, IMapper mapper) 
            : base(propertyTypeRepo, mapper)
        { 
=======
        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepo, IPropertyUnitRepository propertyUnitRepo, IMapper mapper) : base(propertyTypeRepo, mapper)
        {
>>>>>>> 7ef5dd215724d2e6d01d1890e4b2c2f0f9e92cad
            _propertyTypeRepo = propertyTypeRepo;
            _propertyUnitRepo = propertyUnitRepo;
            _mapper = mapper;
        }

<<<<<<< HEAD
        public async Task<List<PropertyTypeDto>> GetAllWithInclude()
        {
            try
            {
                var propertyUnits = await _propertyUnitRepo.GetAllListIncluide(["PropertyUnits"]);
                if (propertyUnits == null)
                {
                    return new List<PropertyTypeDto>();
                }
                return _mapper.Map<List<PropertyTypeDto>>(propertyUnits);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving property units with included data: " + ex.Message);
            }
        }

        public async Task<ResultDto<List<PropertyTypeDto>>> GetAllPropertyType() 
=======
        #region List property types by admin 

        public async Task<ResultDto<List<PropertyTypeDto>>> GetAllPropertyType()
>>>>>>> 7ef5dd215724d2e6d01d1890e4b2c2f0f9e92cad
        {
            var result = new ResultDto<List<PropertyTypeDto>>
            {
                Data = new List<PropertyTypeDto>(),
                Message = new List<string>()
            };

            try
            {
                var propertyTypes = await _propertyTypeRepo.GetAllListAsync();
                if (!propertyTypes.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron tipos de propiedad");
                    return result;
                }

                var propertyTypeList = _mapper.Map<List<PropertyTypeDto>>(propertyTypes);

                foreach (var dto in propertyTypeList)
                {
                    var property = await _propertyUnitRepo.GetAllQueryAsync().Where(p => p.PropertyTypeId == dto.Id).CountAsync();
                    dto.CountProperty = property;
                }

                result.Data = propertyTypeList;

            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }


        #endregion

    }
}
