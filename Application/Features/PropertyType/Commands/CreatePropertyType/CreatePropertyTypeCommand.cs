using MediatR;
using RealState.Core.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.PropertyType.Commands.CreatePropertyType
{
    public class CreatePropertyTypeCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _repo;

        public CreatePropertyTypeCommandHandler(IPropertyTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.PropertyType entity = new()
            { 
                Id = 0,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.PropertyType? result = await _repo.AddAsync(entity);

            return result != null ? result.Id : 0;
        }
    }
}
