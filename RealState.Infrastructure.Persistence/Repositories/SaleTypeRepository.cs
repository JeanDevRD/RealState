using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        public SaleTypeRepository(RealStateContextSql context) : base(context)
        {
        }

    }
}
