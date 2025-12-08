using MediatR;
using RealState.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealState.Core.Application.Features.SaleType.Commands.UpdateSaleType
{
    /// <summary>
    /// parametros para actualizar un tipo de pago
    /// </summary>
    public class UpdateSaleTypeCommand : IRequest<Unit>
    {
        ///<example>1</example>
        public required int Id { get; set; }
        [SwaggerParameter(Description = "Nuevo nombre del tipo de venta")]
        public required string Name { get; set; }

        [SwaggerParameter(Description = "Nueva descripcion del tipo de venta")]
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
