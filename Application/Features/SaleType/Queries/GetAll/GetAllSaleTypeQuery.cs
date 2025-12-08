using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.SaleType.Queries.GetAll
{
    /// <summary>
    /// listar todos los tipos de venta
    /// </summary>
    public class GetAllSaleTypeQuery : IRequest<IList<SaleTypeDto>>
    {
    }

    public class GetAllSaleTypeQueryHandler : IRequestHandler<GetAllSaleTypeQuery, IList<SaleTypeDto>>
    {
        private readonly ISaleTypeRepository _repo;
        private readonly IPropertyUnitRepository _repoProperty;
        private readonly IMapper _mapper;

        public GetAllSaleTypeQueryHandler(ISaleTypeRepository repo, IMapper mapper, IPropertyUnitRepository repoProperty)
        {
            _repo = repo;
            _mapper = mapper;
            _repoProperty = repoProperty;
        }

        public async Task<IList<SaleTypeDto>> Handle(GetAllSaleTypeQuery query, CancellationToken cancellationToken)
        {

            var saleTypes = await _repo.GetAllListAsync();
            if (!saleTypes.Any())
            {
                return [];
            }

            var saleTypeList = _mapper.Map<List<SaleTypeDto>>(saleTypes);

            foreach (var dto in saleTypeList)
            {
                var property = await _repoProperty.GetAllQueryAsync().Where(p => p.PropertyTypeId == dto.Id).CountAsync();
                dto.CountProperty = property;
            }
            return saleTypeList;
        }


    }
}
