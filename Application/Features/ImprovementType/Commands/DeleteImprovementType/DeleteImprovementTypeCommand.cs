using MediatR;
using RealState.Core.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.ImprovementType.Commands.DeleteImprovementType
{
    public class DeleteImprovementTypeCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
    }

    public class DeleteImprovementTypeCommandHandler : IRequestHandler<DeleteImprovementTypeCommand, Unit>
    {
        private readonly IImprovementTypeRepository _repo;

        public DeleteImprovementTypeCommandHandler(IImprovementTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteImprovementTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.ImprovementType? saleType = await _repo.GetByIdAsync(command.Id);

            if (saleType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PAGO");

            await _repo.DeleteAsync(saleType.Id);
            return Unit.Value;
        }
    }
}
