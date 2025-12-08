using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.PropertyUnit.Queries.GetByIdCode
{
    /// <summary>
    /// obtener propiedad por codigo
    /// </summary>
    public class GetByCodePropertyUnitQuery : IRequest<PropertyUnitDto>
    {
        ///<example>1001</example>
        [SwaggerParameter(Description = "introducir codigo")]
        public required int Code { get; set; }
    }

    public class GetByCodePropertyUnitQueryHandler : IRequestHandler<GetByCodePropertyUnitQuery, PropertyUnitDto>
    {
        private readonly IImprovementTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetByCodePropertyUnitQueryHandler(IImprovementTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PropertyUnitDto> Handle(GetByCodePropertyUnitQuery query, CancellationToken cancellationToken)
        {

            var property = await _repo.GetByIdAsync(query.Code);
            if (property == null)
                throw new ArgumentException($"NO SE ENCONTRO la propiedad");

            var result = _mapper.Map<PropertyUnitDto>(property);
            return result;
        }

        
    }
}
