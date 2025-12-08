using MediatR;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Features.SaleType.Commands.CreateSaleType
{
    public class CreateSaleTypeCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _repo;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.SaleType entity = new()
            {
                Id = 0,
                Name = command.Name,
                Description = command.Description
            };

            Domain.Entities.SaleType? result = await _repo.AddAsync(entity);

            return result != null ? result.Id : 0;
        }
    }
}
