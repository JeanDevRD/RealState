using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.ImprovementType.Queries.GetById
{
    public class GetByIdImproventTypeQuery : IRequest<ImprovementTypeDto>
    {
        public required int Id { get; set; }
    }

    public class GetByIdImproventTypeQueryHandler : IRequestHandler<GetByIdImproventTypeQuery, ImprovementTypeDto>
    {
        private readonly IImprovementTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetByIdImproventTypeQueryHandler(IImprovementTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ImprovementTypeDto> Handle(GetByIdImproventTypeQuery query, CancellationToken cancellationToken)
        {

            var improventTypes = await _repo.GetByIdAsync(query.Id);
            if (improventTypes == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE mejora");

            var improventType = _mapper.Map<ImprovementTypeDto>(improventTypes);
            return improventType;
        }

        
    }
}
