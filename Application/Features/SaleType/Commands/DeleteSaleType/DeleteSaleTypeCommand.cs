using MediatR;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.SaleType.Commands.DeleteSaleType
{
    public class DeleteSaleTypeCommand : IRequest<Unit>
    {
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
