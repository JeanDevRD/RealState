using MediatR;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.ImprovementType.Commands.UpdateImprovementType
{
    public class UpdateImprovementTypeCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

    public class UpdateImprovementTypeCommandHeadler : IRequestHandler<UpdateImprovementTypeCommand, Unit>
    {
        private readonly IImprovementTypeRepository _repo;
        public UpdateImprovementTypeCommandHeadler(IImprovementTypeRepository repo)
        {
            _repo = repo;
        }
        public async Task<Unit> Handle(UpdateImprovementTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.ImprovementType? entity = new()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.ImprovementType? saleType = await _repo.GetByIdAsync(command.Id);
            if (saleType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE MEJORA");

            await _repo.UpdateAsync(entity, command.Id);
            return Unit.Value;

        }
    }
}
