using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(RealStateContextSql context) : base(context)
        {
        }
    }
}
