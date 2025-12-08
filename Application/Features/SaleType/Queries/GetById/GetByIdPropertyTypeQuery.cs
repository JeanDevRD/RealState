using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.SaleType.Queries.GetById
{
    public class GetByIdSaleTypeQuery : IRequest<SaleTypeDto>
    {
        public required int Id { get; set; }
    }

    public class GetByIdSaleTypeQueryHandler : IRequestHandler<GetByIdSaleTypeQuery, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _repo;
        private readonly IPropertyUnitRepository _repoProperty;
        private readonly IMapper _mapper;

        public GetByIdSaleTypeQueryHandler(ISaleTypeRepository repo, IMapper mapper, IPropertyUnitRepository repoProperty)
        {
            _repo = repo;
            _mapper = mapper;
            _repoProperty = repoProperty;
        }

        public async Task<SaleTypeDto> Handle(GetByIdSaleTypeQuery query, CancellationToken cancellationToken)
        {

            var saleTypes = await _repo.GetByIdAsync(query.Id);
            if (saleTypes == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE pago");

            var saleType = _mapper.Map<SaleTypeDto>(saleTypes);
            var property = await _repoProperty.GetAllQueryAsync().Where(p => p.PropertyTypeId == saleType.Id).CountAsync();
            saleType.CountProperty = property;
            return saleType;
        }

        
    }
}
