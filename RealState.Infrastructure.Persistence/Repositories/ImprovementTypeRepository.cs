using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class ImprovementTypeRepository : GenericRepository<ImprovementType>, IImprovementTypeRepository    
    {
        public ImprovementTypeRepository(RealStateContextSql context) : base(context)
        {
        }

    }
}
