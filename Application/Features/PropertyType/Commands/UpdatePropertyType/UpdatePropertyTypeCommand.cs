using MediatR;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.PropertyType.Commands.UpdatePropertyType
{
    /// <summary>
    /// parametros para actualizar un tipo de propiedad
    /// </summary>
    public class UpdatePropertyTypeCommand : IRequest<Unit>
    {
        ///<example>1</example>
        public required int Id { get; set; }
        [SwaggerParameter(Description = "Nuevo nombre del tipo de pripiedad")]
        public required string Name { get; set; }

        [SwaggerParameter(Description = "Nueva descripcion del tipo de pripiedad")]
        public required string Description { get; set; }
    }

    public class UpdatePropertyTypeCommandHeadler : IRequestHandler<UpdatePropertyTypeCommand, Unit>
    {
        private readonly IPropertyTypeRepository _repo;
        public UpdatePropertyTypeCommandHeadler(IPropertyTypeRepository repo)
        {
            _repo = repo;
        }
        public async Task<Unit> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.PropertyType? entity = new()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.PropertyType? propertyType = await _repo.GetByIdAsync(command.Id);
            if (propertyType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PROPIEDAD");

            await _repo.UpdateAsync(entity, command.Id);
            return Unit.Value;

        }
    }
}
