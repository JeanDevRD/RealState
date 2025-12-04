using AutoMapper;
using RealState.Core.Application.DTOs.UserFavoritePropertyUnit;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.EntityToDto
{
    public class UserFavoritePropertyUnitMappingProfile : Profile
    {
        public UserFavoritePropertyUnitMappingProfile()
        {
            CreateMap<UserFavoritePropertyUnit, UserFavoritePropertyUnitDto>().ReverseMap();
        }   
    }
}
