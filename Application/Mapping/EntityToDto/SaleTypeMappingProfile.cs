using AutoMapper;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Domain.Entities;
namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class SaleTypeMappingProfile : Profile
    {
        public SaleTypeMappingProfile()
        {
            CreateMap<SaleType, SaleTypeDto>().ReverseMap();
        }
    }
}
