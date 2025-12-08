using AutoMapper;
using MediatR;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.PropertyUnit.Queries.GetById
{
    /// <summary>
    /// obtener propiedad por id
    /// </summary>
    public class GetByIdPropertyUnitQuery : IRequest<PropertyUnitDto>
    {
        ///<example>1</example>
        [SwaggerParameter(Description = "Id de la propiedad")]
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
