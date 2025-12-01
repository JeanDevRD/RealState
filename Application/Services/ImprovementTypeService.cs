using AutoMapper;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class ImprovementTypeService : GenericService<ImprovementType, ImprovementTypeDto>
    {
        public ImprovementTypeService(IGenericRepository<ImprovementType> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
