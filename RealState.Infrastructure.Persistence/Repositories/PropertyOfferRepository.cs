using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class PropertyOfferRepository : GenericRepository<PropertyOffer>, IPropertyOfferRepository
    {
        public PropertyOfferRepository(RealStateContextSql context) : base(context)
        {
        }

    }
}
