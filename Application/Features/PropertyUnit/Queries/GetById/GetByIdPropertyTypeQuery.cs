using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.PropertyUnit.Queries.GetById
{
    public class GetByIdPropertyUnitQuery : IRequest<PropertyUnitDto>
    {
        public required int Id { get; set; }
    }

    public class GetByIdPropertyUnitQueryHandler : IRequestHandler<GetByIdPropertyUnitQuery, PropertyUnitDto>
    {
        private readonly IImprovementTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetByIdPropertyUnitQueryHandler(IImprovementTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PropertyUnitDto> Handle(GetByIdPropertyUnitQuery query, CancellationToken cancellationToken)
        {

            var property = await _repo.GetByIdAsync(query.Id);
            if (property == null)
                throw new ArgumentException($"NO SE ENCONTRO la propiedad");

            var result = _mapper.Map<PropertyUnitDto>(property);
            return result;
        }


    }
}
