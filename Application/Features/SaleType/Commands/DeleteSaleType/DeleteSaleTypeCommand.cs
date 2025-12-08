using MediatR;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace RealState.Core.Application.Features.SaleType.Commands.DeleteSaleType
{
    /// <summary>
    /// parametros para eliminar un tipo de pago
    /// </summary>
    public class DeleteSaleTypeCommand : IRequest<Unit>
    {
        ///<example>1</example>
        [SwaggerParameter(Description = "Id del tipo de pago")]
        public required int Id { get; set; }
    }

    public class DeleteSaleTypeCommandHandler : IRequestHandler<DeleteSaleTypeCommand, Unit>
    {
        private readonly ISaleTypeRepository _repo;

        public DeleteSaleTypeCommandHandler(ISaleTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteSaleTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.SaleType? saleType = await _repo.GetByIdAsync(command.Id);

            if (saleType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PAGO");

            await _repo.DeleteAsync(saleType.Id);
            return Unit.Value;
        }
    }
}
