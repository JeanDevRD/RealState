using MediatR;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.ImprovementType.Commands.CreateImprovementType
{
    public class CreateImprovementTypeCommand : IRequest<int>
    {
        public required string Name { get; set; }
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
