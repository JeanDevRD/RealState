using AutoMapper;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Application.ViewModels.Admin;
using RealState.Core.Application.ViewModels.Agent;
using RealState.Core.Application.ViewModels.Developer;
using RealState.Core.Application.ViewModels.PropertyType;
using RealState.Core.Application.ViewModels.SalesType;
using RealState.Core.Domain.Entities;

namespace RealState.Core.Application.Mapping.DtoToViewModel
{
    public class SaleTypeViewModelMappingProfile : Profile
    {
        public SaleTypeViewModelMappingProfile()
        {
            CreateMap<SaleTypeDto, SaleTypeViewModel>()
                .ReverseMap();

            CreateMap<SaleTypeDto, SaveSaleTypeViewModel>()
                .ReverseMap()
                .ForMember(p => p.CountProperty, p => p.Ignore());


            
        }
    }
}
