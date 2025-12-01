using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaleType, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepo;
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepo, IPropertyUnitRepository propertyUnitRepo, IMapper mapper) : base(saleTypeRepo, mapper)
        {
            _saleTypeRepo = saleTypeRepo;
            _propertyUnitRepo = propertyUnitRepo;
            _mapper = mapper;
        }

        #region List sale types by admin 

        public async Task<ResultDto<List<SaleTypeDto>>> GetAllSaleType()
        {
            var result = new ResultDto<List<SaleTypeDto>>
            {
                Data = new List<SaleTypeDto>(),
                Message = new List<string>()
            };

            try
            {
                var saleTypes = await _saleTypeRepo.GetAllListAsync();
                if (!saleTypes.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron tipos de ventas");
                    return result;
                }

                var saleTypeList = _mapper.Map<List<SaleTypeDto>>(saleTypes);

                foreach (var dto in saleTypeList)
                {
                    var property = await _propertyUnitRepo.GetAllQueryAsync().Where(p => p.PropertyTypeId == dto.Id).CountAsync();
                    dto.CountProperty = property;
                }

                result.Data = saleTypeList;

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
