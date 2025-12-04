using AutoMapper;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class ImprovementTypeMappingProfile : Profile
    {
        public ImprovementTypeMappingProfile()
        {
            CreateMap<ImprovementType, ImprovementTypeDto>().ReverseMap();
        }
    }
}
