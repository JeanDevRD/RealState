using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.ImprovementType.Queries.GetAll
{
    public class GetAllImprovementTypeQuery : IRequest<IList<ImprovementTypeDto>>
    {
    }

    public class GetAllImprovementTypeQueryHandler : IRequestHandler<GetAllImprovementTypeQuery, IList<ImprovementTypeDto>>
    {
        private readonly ISaleTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetAllImprovementTypeQueryHandler(ISaleTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IList<ImprovementTypeDto>> Handle(GetAllImprovementTypeQuery query, CancellationToken cancellationToken)
        {

            var improvent = await _repo.GetAllListAsync();
            if (!improvent.Any())
            {
                return [];
            }

            var improventTypeList = _mapper.Map<List<ImprovementTypeDto>>(improvent);
            return improventTypeList;
        }


    }
}
