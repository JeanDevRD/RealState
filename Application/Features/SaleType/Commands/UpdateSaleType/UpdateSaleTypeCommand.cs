using MediatR;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.SaleType.Commands.UpdateSaleType
{
    public class UpdateSaleTypeCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

    public class UpdateSaleTypeCommandHeadler : IRequestHandler<UpdateSaleTypeCommand, Unit>
    {
        private readonly ISaleTypeRepository _repo;
        public UpdateSaleTypeCommandHeadler(ISaleTypeRepository repo)
        {
            _repo = repo;
        }
        public async Task<Unit> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.SaleType? entity = new()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.SaleType? saleType = await _repo.GetByIdAsync(command.Id);
            if (saleType == null)
                throw new ArgumentException($"NO SE ENCONTRO EL TIPO DE PAGO");

            await _repo.UpdateAsync(entity, command.Id);
            return Unit.Value;

        }
    }
}
