using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyType,PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepo;
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepo, IPropertyUnitRepository propertyUnitRepo, IMapper mapper) 
            : base(propertyTypeRepo, mapper)
        { 
            _propertyTypeRepo = propertyTypeRepo;
            _propertyUnitRepo = propertyUnitRepo;
            _mapper = mapper;
        }

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

                foreach (var propertyType in propertyTypes) 
                {
                    var property = await _propertyUnitRepo.GetAllQueryAsync().Where(p => p.Id == propertyType.Id).CountAsync();
                    
                }

            }
            catch (Exception ex) 
            { 
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }
    }
}
