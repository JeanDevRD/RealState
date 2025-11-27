using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class PropertyUnitRepository : GenericRepository<PropertyUnit>, IPropertyUnitRepository
    {
        public PropertyUnitRepository(RealStateContextSql context) : base(context)
        {
        }

    }
}
