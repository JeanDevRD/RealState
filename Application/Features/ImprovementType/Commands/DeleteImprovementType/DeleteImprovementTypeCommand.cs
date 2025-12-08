using MediatR;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.ImprovementType.Commands.DeleteImprovementType
{
    /// <summary>
    /// parametros para eliminar un tipo de mejora
    /// </summary>
    public class DeleteImprovementTypeCommand : IRequest<Unit>
    {
        ///<example>1</example>
        [SwaggerParameter("Id de la mejora ")]
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
