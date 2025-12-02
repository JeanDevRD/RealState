using AutoMapper;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class ImprovementTypeService : GenericService<ImprovementType, ImprovementTypeDto>
    {
      private readonly IImprovementTypeRepository _improvementTypeRepo;
      private readonly IMapper _mapper;

        public ImprovementTypeService(IImprovementTypeRepository improvementTypeRepo, IMapper mapper) 
            : base(improvementTypeRepo, mapper)
        {
            _improvementTypeRepo = improvementTypeRepo;
            _mapper = mapper;
        }

        public async Task<List<ImprovementTypeDto>> GetAllWithInclude()
        {
            try
            {
                var improvementTypes = await _improvementTypeRepo.GetAllListIncluide(["PropertyUnits"]);
                if (improvementTypes == null)
                {
                    return new List<ImprovementTypeDto>();
                }
                return _mapper.Map<List<ImprovementTypeDto>>(improvementTypes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving improvement types with included data: " + ex.Message);
            }
        }

        public async Task<ResultDto<List<ImprovementTypeDto>>> GetAllImprovementTypes() 
        {
            var result = new ResultDto<List<ImprovementTypeDto>>
            {
                Data = new List<ImprovementTypeDto>(),
                Message = new List<string>()
            };

            try
            {
                var improvementTypes = await _improvementTypeRepo.GetAllListAsync();
                if (!improvementTypes.Any()) 
                { 
                    result.IsError = true;
                    result.Message.Add("No se encontraron tipos de mejora");
                    return result;
                }

                result.Data = _mapper.Map<List<ImprovementTypeDto>>(improvementTypes);
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
