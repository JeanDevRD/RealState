using MediatR;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.ImprovementType.Commands.CreateImprovementType
{
    /// <summary>
    /// parametros para crear un tipo de mejora
    /// </summary>
    public class CreateImprovementTypeCommand : IRequest<int>
    {
        ///<example>Piscina</example>
        [SwaggerParameter(Description = "Nombre del tipo de mejora")]
        public required string Name { get; set; }

        [SwaggerParameter(Description = "Descripcion del tipo de mejora")]
        public required string Description { get; set; }
    }

    public class CreateImprovementTypeCommandHandler : IRequestHandler<CreateImprovementTypeCommand, int>
    {
        private readonly IImprovementTypeRepository _repo;

        public CreateImprovementTypeCommandHandler(IImprovementTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateImprovementTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.ImprovementType entity = new()
            {
                Id = 0,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.ImprovementType? result = await _repo.AddAsync(entity);

            return result != null ? result.Id : 0;
        }
    }
}
