using MediatR;
using RealState.Core.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.PropertyType.Commands.CreatePropertyType
{
    public class DeletePropertyTypeCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
    }

    public class DeletePropertyTypeCommandHandler : IRequestHandler<DeletePropertyTypeCommand, Unit>
    {
        private readonly IPropertyTypeRepository _repo;

        public DeletePropertyTypeCommandHandler(IPropertyTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeletePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.PropertyType? propertyType = await _repo.GetByIdAsync(command.Id);

            if (propertyType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PROPIEDAD");

            await _repo.DeleteAsync(propertyType.Id);
            return Unit.Value;
        }
    }
}
