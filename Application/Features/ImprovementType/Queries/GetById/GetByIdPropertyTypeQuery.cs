using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.ImprovementType.Queries.GetById
{
    /// <summary>
    /// obtener tipo de mejora por id
    /// </summary>
    public class GetByIdImproventTypeQuery : IRequest<ImprovementTypeDto>
    {
        ///<example>1</example>
        [SwaggerParameter(Description = "Id del tipo de mejora")]
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
